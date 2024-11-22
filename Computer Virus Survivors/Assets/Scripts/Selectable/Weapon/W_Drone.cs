using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class W_Drone : WeaponBehaviour
{
    [SerializeField] private float altitude = 3f;
    [SerializeField] private float dronePosRadius = 1f;
    private List<GameObject> hangingPoints = new List<GameObject>();
    private List<P_Drone> drones = new List<P_Drone>();
    private int droneCount = 0;
    protected override IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitUntil(() => droneCount < finalMultiProjectile);

            SpawnDrone();

        }
    }

    private void SpawnDrone()
    {
        GameObject newPoint = new GameObject();
        newPoint.transform.SetParent(transform);
        hangingPoints.Add(newPoint);

        ReplaceHangingPoints();

        GameObject newDrone = PoolManager.instance.GetObject(projectilePool, transform.position, transform.rotation);
        newDrone.GetComponent<P_Drone>().Initialize(finalDamage, finalAttackRange, newPoint.transform, finalAttackPeriod);
        drones.Add(newDrone.GetComponent<P_Drone>());

        droneCount++;
    }

    private void ReplaceHangingPoints()
    {
        int count = hangingPoints.Count;

        if (count == 1)
        {
            hangingPoints[0].transform.localPosition = new Vector3(0, altitude, 0);
        }
        else
        {
            float angle = 2 * Mathf.PI / count;
            for (int i = 0; i < count; i++)
            {
                hangingPoints[i].transform.localPosition = new Vector3(dronePosRadius * Mathf.Cos(angle * i), altitude, dronePosRadius * Mathf.Sin(angle * i));
            }
        }
    }

    private void UpdateDrone()
    {
        for (int i = 0; i < droneCount; i++)
        {
            drones[i].UpgradeDrone(finalDamage, finalAttackRange, finalAttackPeriod);
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
                BasicDamage += 5;
                break;
            case 3:
                BasicAttackPeriod *= (1 - 0.3f) / 1;
                break;
            case 4:
                BasicAttackRange *= 1.5f;
                break;
            case 5:
                BasicDamage += 10;
                break;
            case 6:
                BasicMultiProjectile += 1;
                break;
            case 7:
                BasicAttackPeriod *= (1 - 0.3f) / 1;
                break;
            case 8:
                BasicDamage += 10;
                BasicAttackRange *= 1.5f;
                break;
            case 9:
                BasicMultiProjectile += 1;
                break;
            default:
                break;
        }

        UpdateDrone();
    }

    protected override void InitExplanation()
    {
        switch (MaxLevel)
        {
            case 1:
                explanations[0] = "플레이어를 따라다니는 드론을 소환합니다. 드론은 가장 가까운 적을 자동 공격합니다.";
                break;
            case 2:
                explanations[1] = "드론 데미지 5 증가";
                goto case 1;
            case 3:
                explanations[2] = "드론 공격 주기 30% 감소";
                goto case 2;
            case 4:
                explanations[3] = "드론 공격 반경 50% 증가";
                goto case 3;
            case 5:
                explanations[4] = "드론 데미지 10 증가";
                goto case 4;
            case 6:
                explanations[5] = "드론 1기 추가";
                goto case 5;
            case 7:
                explanations[6] = "드론 공격 주기 30% 감소";
                goto case 6;
            case 8:
                explanations[7] = "드론 데미지 10 증가.\n드론 공격 반경 50% 증가";
                goto case 7;
            case 9:
                explanations[8] = "드론 1기 추가";
                goto case 8;
        }
    }




}

