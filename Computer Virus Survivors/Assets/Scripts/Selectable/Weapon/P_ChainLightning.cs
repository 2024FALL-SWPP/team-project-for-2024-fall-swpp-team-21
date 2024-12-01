using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;
using System.Linq;


public class P_ChainLightning : PlayerProjectileBehaviour
{
    [SerializeField] private LayerMask virusLayer;
    [SerializeField] private float chainInterval;
    [SerializeField] private float chainDuration;
    [SerializeField] private Vector3 chainHitOffset;

    private LightningBoltScript chainAnim;
    private ParticleSystem hitEffect;
    private int chainID;
    private int chainDepth;
    private float chainRange;
    private int branchCount;
    private VirusBehaviour targetVirus;

    public void Initialize(FinalWeaponData finalWeaponData, int chainID, float chainRange, int chainDepth, int branchCount, VirusBehaviour iniitalVirus = null)
    {
        base.Initialize(finalWeaponData);
        this.chainID = chainID;
        this.chainRange = chainRange;
        this.chainDepth = chainDepth;
        this.branchCount = branchCount;
        this.targetVirus = iniitalVirus;

        if (chainAnim == null)
        {
            animator = GetComponent<Animator>();
            chainAnim = GetComponentInChildren<LightningBoltScript>();
            hitEffect = GetComponentInChildren<ParticleSystem>();
        }

        chainAnim.StartPosition = transform.position;

        if (chainDepth <= 0)
        {
            PoolManager.instance.ReturnObject(PoolType.Proj_ChainLightning, gameObject);
        }
        else
        {
            StartCoroutine(ChainLightning());
        }
    }


    /// <summary>
    /// 하나의 번개는 하나의 적만 공격
    /// 공격한 몬스터에서 다음 공격이 갈라져 나감
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChainLightning()
    {
        yield return new WaitForSeconds(chainInterval);

        // 같은 번개 가지로부터 공격 받지 않은 범위 내 랜덤한 몬스터 찾음
        if (targetVirus == null)
        {

            targetVirus = GetNextVirus();
        }

        Debug.Log(targetVirus);
        // 그런 몬스터가 없으면 번개 삭제
        if (targetVirus == null)
        {
            PoolManager.instance.ReturnObject(PoolType.Proj_ChainLightning, gameObject);
            yield break;
        }

        // 해당 몬스터에서 branch개의 번개 생성
        for (int i = 0; i < branchCount; i++)
        {
            GameObject chainLightning = PoolManager.instance.GetObject(PoolType.Proj_ChainLightning, targetVirus.transform.position + chainHitOffset, Quaternion.identity);
            P_ChainLightning chainLightningScript = chainLightning.GetComponent<P_ChainLightning>();
            chainLightningScript.Initialize(finalWeaponData, chainID, chainRange, chainDepth - 1, branchCount);
        }

        // 해당 몬스터를 공격 (애니메이션)
        chainAnim.EndPosition = targetVirus.transform.position + chainHitOffset;
        Vector3 emissionDirection = transform.position - targetVirus.transform.position;
        hitEffect.transform.rotation = Quaternion.LookRotation(emissionDirection);
        hitEffect.transform.position = chainAnim.EndPosition;
        hitEffect.Play();
        animator.SetBool("LightOn_b", true);
        targetVirus.GetDamage(finalWeaponData.GetFinalDamage(), finalWeaponData.knockbackTime);
        yield return new WaitForSeconds(chainDuration);
        animator.SetBool("LightOn_b", false);

        // 삭제
        PoolManager.instance.ReturnObject(PoolType.Proj_ChainLightning, gameObject);
    }

    private VirusBehaviour GetNextVirus()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, chainRange, virusLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            int randomIndex = Random.Range(i, colliders.Length);

            // 두 요소를 스왑
            (colliders[randomIndex], colliders[i]) = (colliders[i], colliders[randomIndex]);
        }

        List<GameObject> viruses = colliders.Select(collider => collider.gameObject)
            .ToList();

        foreach (GameObject virusObject in viruses)
        {
            ChainLightningMarker marker = virusObject.GetComponent<ChainLightningMarker>();
            if (marker == null)
            {
                marker = virusObject.AddComponent<ChainLightningMarker>();
            }

            if (marker.IsNotStrucked(chainID))
            {
                VirusBehaviour ret = virusObject.GetComponent<VirusBehaviour>();
                ret.OnDie += marker.OnVirusDied;
                return ret;
            }
        }

        return null;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // do nothing
    }

}


// 같은 번개 가지로부터 공격 받은 몬스터는 마킹 해두어 다시 공격 받지 않도록 함
// 공격 받으면 몬스터에 추가되는 컴포넌트
// 이 컴포넌트는 한 번 추가되면 삭제되지 않음
public class ChainLightningMarker : MonoBehaviour
{
    public List<int> chainID;

    public bool IsNotStrucked(int chainID)
    {
        if (this.chainID == null)
        {
            this.chainID = new List<int>();
        }

        if (this.chainID.Contains(chainID))
        {
            return false;
        }
        else
        {
            this.chainID.Add(chainID);
            StartCoroutine(ResetChainID(chainID));
            return true;
        }
    }

    // 일단 0.5초로 하긴 했는데 이 시간은 조절 가능
    // 플레이어 번개 공격 주기로 하면 딱 맞을 듯
    private IEnumerator ResetChainID(int chainID)
    {
        yield return new WaitForSeconds(0.5f);
        this.chainID.Remove(chainID);
    }

    public void OnVirusDied(VirusBehaviour virus)
    {
        StopAllCoroutines();
        chainID.Clear();
    }
}
