using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;
using System.Linq;

public class P_ChainLightning : MonoBehaviour
{
    [SerializeField] private LayerMask virusLayer;
    [SerializeField] private float chainInterval;
    [SerializeField] private float chainDuration;
    [SerializeField] private Vector3 chainHitOffset;

    private Animator animator;
    private LightningBoltScript chainAnim;
    private int damage;
    private int chainID;
    private int chainDepth;
    private float chainRange;
    private int branchCount;

    public void Initialize(int damage, int chainID, float chainRange, int chainDepth, int branchCount)
    {
        this.chainID = chainID;
        this.chainRange = chainRange;
        this.chainDepth = chainDepth;
        this.damage = damage;
        this.branchCount = branchCount;

        animator = GetComponent<Animator>();
        chainAnim = GetComponentInChildren<LightningBoltScript>();
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
        VirusBehaviour targetVirus = GetNextVirus();
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
            chainLightningScript.Initialize(damage, chainID, chainRange, chainDepth - 1, branchCount);
        }

        // 해당 몬스터를 공격 (애니메이션)
        chainAnim.EndPosition = targetVirus.transform.position + chainHitOffset;
        animator.SetBool("LightOn_b", true);
        targetVirus.GetDamage(damage);
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
                return virusObject.GetComponent<VirusBehaviour>();
            }
        }

        return null;
    }

}

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

    private IEnumerator ResetChainID(int chainID)
    {
        yield return new WaitForSeconds(0.5f);
        this.chainID.Remove(chainID);
    }
}
