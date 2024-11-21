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
                float angle = proj.GetComponent<P_Shield>().GetCurrentAngle();
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
                BasicAttackRange *= (1 + 0.25f) / 1;
                BasicAttackPeriod *= 1 / (1 + 0.3f);
                break;
            case 4:
                BasicDamage += 10;
                break;
            case 5:
                BasicMultiProjectile += 1;
                break;
            case 6:
                BasicAttackRange *= (1 + 0.25f) / 1;
                BasicAttackPeriod *= 1 / (1 + 0.3f);
                break;
            case 7:
                BasicDamage += 10;
                break;
            case 8:
                BasicMultiProjectile += 1;
                break;
            case 9:
                BasicMultiProjectile += 2;
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
                explanations[0] = "백신 방패가 플레이어 주위를 회전하면서 닿은 바이러스에게 데미지를 입힙니다";
                break;
            case 2:
                explanations[1] = "백신 방패 1개 추가";
                goto case 1;
            case 3:
                explanations[2] = "공격 범위 25% 증가, 회전 속도 30% 증가";
                goto case 2;
            case 4:
                explanations[3] = "기본 데미지 10 증가";
                goto case 3;
            case 5:
                explanations[4] = "백신 방패 1개 추가";
                goto case 4;
            case 6:
                explanations[5] = "공격 범위 25% 증가, 회전 속도 30% 증가";
                goto case 5;
            case 7:
                explanations[6] = "기본 데미지 10 증가";
                goto case 6;
            case 8:
                explanations[7] = "백신 방패 1개 추가";
                goto case 7;
            case 9:
                explanations[8] = "백신 방패 2개 추가";
                goto case 8;
        }
    }

    private void InitializeProjectiles()
    {
        Debug.Log(finalMultiProjectile);
        if (projs.Count < finalMultiProjectile)
        {
            while (projs.Count < finalMultiProjectile)
            {
                projs.Add(PoolManager.instance.GetObject(projectilePool, transform.position, Quaternion.identity));
            }

            for (int i = 0; i < projs.Count; i++)
            {
                GameObject proj = projs[i];
                float angle = 2f * MathF.PI / projs.Count * i;
                proj.GetComponent<P_Shield>().Initialize(finalDamage, angle, 2f * MathF.PI / finalAttackPeriod);
            }
        }
        else
        {
            foreach (GameObject proj in projs)
            {
                proj.GetComponent<P_Shield>().Initialize(finalDamage, -1, 2f * MathF.PI / finalAttackPeriod);
            }
        }
    }
}
