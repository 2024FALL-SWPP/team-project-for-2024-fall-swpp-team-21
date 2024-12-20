using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class W_ChainLightning : WeaponBehaviour
{
    [SerializeField] private LayerMask virusLayer;
    [SerializeField] private float attackRadius;
    [SerializeField] private float chainRadius;
    [SerializeField] private int chainDepth;
    [SerializeField] private int branchCount;
    [SerializeField] private Vector3 lightningStartOffset;
    [SerializeField] private SFXPreset shootSFX;

    protected override IEnumerator Attack()
    {
        while (true)
        {
            Debug.Log("Shoot!");
            StartCoroutine(Shoot());
            yield return new WaitForSeconds(finalWeaponData.attackPeriod);
        }
    }

    protected override void LevelUpEffect(int level)
    {
        switch (level)
        {
            case 1:
                // Do nothing
                break;
            case 2:
                BasicMultiProjectile += 1;
                break;
            case 3:
                branchCount += 1;
                break;
            case 4:
                BasicDamage += 1;
                chainDepth += 1;
                break;
            case 5:
                BasicDamage += 1;
                chainRadius *= 1.5f;
                break;
            case 6:
                BasicDamage += 2;
                BasicMultiProjectile += 1;
                break;
            case 7:
                chainDepth += 1;
                break;
            case 8:
                BasicMultiProjectile += 1;
                BasicDamage += 2;
                break;
            case 9:
                BasicMultiProjectile += 1;
                BasicDamage += 2;
                break;
            default:
                break;
        }
    }

    // 가지 개수와 전이 횟수는 예민하게 다뤄야 할 문제인듯
    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "번개의 형상을 띈 바이러스 탐지기가 적들을 추적하여 공격합니다\n번개는 바이러스를 통해 전이될 수 있습니다";
                // total damage = basicDamage * basicMultiProjectile * (1 + branch ^ chainDepth) (1 * 1* (1 + 2 ^ 1) = 3)
                break;
            case 2:
                explanations[1] = "번개 <color=#FF00C7>1</color>개 증가";
                // total = (1 * 2 * (1 + 2 ^ 1) = 6)
                goto case 1;
            case 3:
                explanations[2] = "번개 가지 <color=#FF00C7>1</color>개 증가";
                // total = (1 * 2 * (1 + 3 ^ 1) = 9)
                goto case 2;
            case 4:
                explanations[3] = "기본 데미지 <color=#FF00C7>1 증가, 추가 전이 횟수 <color=#FF00C7>1</color>회 증가";
                // total = (2 * 2 * (1 + 3 ^ 2) = 40)
                goto case 3;
            case 5:
                explanations[4] = "기본 데미지 <color=#FF00C7>1 증가, 전이 거리 <color=#FF00C7>50%</color> 증가";
                // total = (3 * 2 * (1 + 3 ^ 2) = 60)
                goto case 4;
            case 6:
                explanations[5] = "기본 데미지 <color=#FF00C7>2 증가, 번개 <color=#FF00C7>1</color>개 증가";
                // total = (5 * 3 * (1 + 3 ^ 2) = 150)
                goto case 5;
            case 7:
                explanations[6] = "추가전이 횟수 <color=#FF00C7>1</color>회 증가";
                // total = 5 * 3 * (1 + 3 ^ 3) = 420)
                goto case 6;
            case 8:
                explanations[7] = "번개 1개 증가, 기본 데미지 <color=#FF00C7>2</color> 증가";
                // total = (7 * 4 * (1 + 3 ^ 3) = 784)
                goto case 7;
            case 9:
                explanations[8] = "번개 1개 증가, 기본 데미지 <color=#FF00C7>2</color> 증가";
                // total = (9 * 5 * (1 + 3 ^ 3) = 1260)
                goto case 8;
        }
    }


    private IEnumerator Shoot()
    {

        for (int chainID = 0; chainID < BasicMultiProjectile; chainID++)
        {
            VirusBehaviour target = GetNextVirus(chainID);
            Debug.Log(target);
            if (target == null)
            {
                yield break;
            }
            Vector3 lightningStart = target.transform.position + lightningStartOffset;

            PoolManager.instance.GetObject(PoolType.Proj_ChainLightning, lightningStart, Quaternion.identity)
                .GetComponent<P_ChainLightning>().Initialize(finalWeaponData, chainID, chainRadius, chainDepth, branchCount, target);
            shootSFX?.Play();
            yield return new WaitForSeconds(0.07f);
        }
    }


    // 공격 범위 내에 있는 몬스터 중에서 공격하지 않은 몬스터를 랜덤하게 반환
    // chainID가 겹치는 일은 없기 때문에 몬스터 한 마리가 여러 번개를 맞을 수도 있음
    private VirusBehaviour GetNextVirus(int chainID)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius, virusLayer);

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

