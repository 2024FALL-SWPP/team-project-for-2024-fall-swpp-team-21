using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_ChainLightning : WeaponBehaviour
{
    [SerializeField] private LayerMask virusLayer;
    [SerializeField] private float attackRadius;
    [SerializeField] private int chainDepth;
    [SerializeField] private int branchCount;

    protected override IEnumerator Attack()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(finalAttackPeriod);
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
                BasicAttackPeriod *= 1 / (1 + 0.15f);
                break;
            case 3:
                BasicDamage += 5;
                break;
            case 4:
                BasicMultiProjectile += 1;
                break;
            case 5:
                BasicDamage += 5;
                break;
            case 6:
                BasicAttackPeriod *= 1 / (1 + 0.15f);
                break;
            case 7:
                BasicAttackPeriod *= 1 / (1 + 0.3f);
                break;
            case 8:
                BasicDamage += 10;
                break;
            case 9:
                BasicAttackPeriod *= 1 / (1 + 0.4f);
                BasicDamage += 10;
                break;
            default:
                break;
        }
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "플레이어의 전방을 향해 빠르게 탄환을 발사합니다";
                break;
            case 2:
                explanations[1] = "공격 속도 15% 증가";
                goto case 1;
            case 3:
                explanations[2] = "기본 데미지 5 증가";
                goto case 2;
            case 4:
                explanations[3] = "투사체 1개 추가 발사";
                goto case 3;
            case 5:
                explanations[4] = "기본 데미지 5 증가";
                goto case 4;
            case 6:
                explanations[5] = "공격 속도 15% 증가";
                goto case 5;
            case 7:
                explanations[6] = "공격 속도 30% 증가";
                goto case 6;
            case 8:
                explanations[7] = "기본 데미지 10 증가";
                goto case 7;
            case 9:
                explanations[8] = "공격 속도 40% 증가, 기본 데미지 10 증가";
                goto case 8;
        }
    }

    private void Shoot()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius, virusLayer);

        for (int chainID = 0; chainID < BasicMultiProjectile; chainID++)
        {

            // Collider target = colliders[Random.Range(0, colliders.Length)];
            // colliders.

            // GameObject storm = PoolManager.instance.GetObject(PoolType.Proj_ChainLightning, transform.position, transform.rotation);

            // storm.GetComponent<P_ChainLightning>().Initialize(finalDamage, chainID, 5, chainDepth, branchCount);
        }

    }
}
