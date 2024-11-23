using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class W_FlameThrower : WeaponBehaviour
{
    [SerializeField] private P_FlameThrower proj;
    [SerializeField] private float totalAttackPeriod; // 공격 시작 ~ 다음 공격 시작

    protected override IEnumerator Attack()
    {
        while (true)
        {
            proj.Initialize(finalWeaponData);
            proj.FireOn(totalAttackPeriod - finalWeaponData.attackPeriod);
            yield return new WaitForSeconds(totalAttackPeriod);
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
                BasicAttackPeriod -= 2;
                break;
            case 3:
                BasicAttackRange *= (1 + 0.15f);
                break;
            case 4:
                BasicDamage += 5;
                break;
            case 5:
                BasicAttackPeriod -= 2;
                break;
            case 6:
                BasicDamage += 5;
                break;
            case 7:
                BasicAttackRange *= (1 + 0.15f);
                break;
            case 8:
                BasicDamage += 10;
                break;
            case 9:
                BasicAttackPeriod = 0;
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
                explanations[0] = "플레이어 전방의 부채꼴 범위로 불을 뿜어 바이러스에게 지속적인 데미지를 줍니다.";
                break;
            case 2:
                explanations[1] = "공격 주기 2초 감소";
                goto case 1;
            case 3:
                explanations[2] = "공격 범위 15% 증가";
                goto case 2;
            case 4:
                explanations[3] = "기본 데미지 5 증가";
                goto case 3;
            case 5:
                explanations[4] = "공격 주기 2초 감소";
                goto case 4;
            case 6:
                explanations[5] = "기본 데미지 5 증가";
                goto case 5;
            case 7:
                explanations[6] = "공격 범위 15% 증가";
                goto case 6;
            case 8:
                explanations[7] = "기본 데미지 10 증가";
                goto case 7;
            case 9:
                explanations[8] = "화염 방사기가 꺼지지 않습니다";
                goto case 8;
        }
    }


}
