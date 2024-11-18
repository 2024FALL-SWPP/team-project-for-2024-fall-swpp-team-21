using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class W_Shield : WeaponBehaviour
{
    [SerializeField] private float shieldHeight;
    private List<GameObject> projs = new List<GameObject>();

    protected override IEnumerator Attack()
    {
        InitializeProjectiles();
        while (true)
        {
            foreach (GameObject proj in projs)
            {
                float angle = proj.GetComponent<P_Shield>().GetCurrentAngle() * Mathf.Deg2Rad;
                Vector2 circlePoint = finalAttackRange * new Vector2(MathF.Cos(angle), MathF.Sin(angle));
                proj.transform.position = transform.position + new Vector3(circlePoint.x, shieldHeight, circlePoint.y);
            }
            yield return null;
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
                BasicAttackPeriod *= (1 - 0.3f) / 1;
                break;
            case 4:
                BasicDamage += 20;
                break;
            case 5:
                BasicMultiProjectile += 1;
                break;
            case 6:
                BasicAttackPeriod *= (1 - 0.3f) / 1;
                break;
            case 7:
                BasicDamage += 40;
                break;
            case 8:
                BasicMultiProjectile += 1;
                break;
            case 9:
                BasicMultiProjectile += 1;
                BasicDamage += 40;
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
                explanations[0] = "플레이어 주위의 랜덤한 위치에 코어가 떨어집니다.\n떨어진 위치 주변의 바이러스들은 피해를 입습니다.";
                break;
            case 2:
                explanations[1] = "떨어지는 코어 1개 추가";
                goto case 1;
            case 3:
                explanations[2] = "공격 주기 30% 감소";
                goto case 2;
            case 4:
                explanations[3] = "기본 데미지 20 증가";
                goto case 3;
            case 5:
                explanations[4] = "떨어지는 코어 1개 추가";
                goto case 4;
            case 6:
                explanations[5] = "공격 주기 30% 감소";
                goto case 5;
            case 7:
                explanations[6] = "기본 데미지 40 증가";
                goto case 6;
            case 8:
                explanations[7] = "떨어지는 코어 1개 추가";
                goto case 7;
            case 9:
                explanations[8] = "떨어지는 코어 1개 증가, 기본 데미지 40 증가";
                goto case 8;
        }
    }

    private void InitializeProjectiles()
    {
        if (projs.Count < finalMultiProjectile)
        {
            for (int i = 0; i < finalMultiProjectile - projs.Count; i++)
            {
                projs.Add(PoolManager.instance.GetObject(projectilePool, transform.position, Quaternion.identity));
            }

            for (int i = 0; i < projs.Count; i++)
            {
                GameObject proj = projs[i];
                float angle = 2f * MathF.PI / projs.Count * i;
                //Vector2 circlePoint = GetCirclePoint(finalAttackRange, i, projs.Count);
                proj.GetComponent<P_Shield>().Initialize(finalDamage, angle, 360f / finalAttackPeriod);
            }
        }
        else
        {
            foreach (GameObject proj in projs)
            {
                proj.GetComponent<P_Shield>().Initialize(finalDamage, -1, 360f / finalAttackPeriod);
            }
        }
    }

    private Vector2 GetCirclePoint(float radius, int step, int totalSteps)
    {
        float angle = 2f * MathF.PI / totalSteps * step;
        return radius * new Vector2(MathF.Cos(angle), MathF.Sin(angle));
    }
}
