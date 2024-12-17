using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class W_CoreDump : WeaponBehaviour
{

    [SerializeField] private float dumpAreaRadius;
    [SerializeField] private float dumpStartHeight;
    [SerializeField] private SFXPreset shootSFX;
    private readonly Vector3[] fallingDirections = { new Vector3(1, -1, 1).normalized, new Vector3(1, -1, -1).normalized,
                                                     new Vector3(-1, -1, 1).normalized, new Vector3(-1, -1, -1).normalized };

    protected override IEnumerator Attack()
    {
        while (true)
        {
            StartCoroutine(Dump());
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
                BasicAttackPeriod *= (1 - 0.15f) / 1;
                break;
            case 4:
                BasicAttackPeriod *= (1 - 0.15f) / 1;
                break;
            case 5:
                BasicMultiProjectile += 1;
                break;
            case 6:
                BasicDamage += 15;
                break;
            case 7:
                BasicMultiProjectile += 1;
                break;
            case 8:
                BasicDamage += 20;
                break;
            case 9:
                BasicMultiProjectile += 1;
                BasicDamage += 30;
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
                explanations[0] = "데이터 코어가 메모리 공간을 백업한 뒤 삭제합니다\n떨어진 위치 주변의 바이러스들을 공격합니다";
                break;
            case 2:
                explanations[1] = "떨어지는 코어 <color=#FF00C7>1</color>개 추가";
                goto case 1;
            case 3:
                explanations[2] = "공격 주기 <color=#FF00C7>15%</color> 감소";
                goto case 2;
            case 4:
                explanations[3] = "공격 주기 <color=#FF00C7>15%</color> 감소";
                goto case 3;
            case 5:
                explanations[4] = "떨어지는 코어 <color=#FF00C7>1</color>개 추가";
                goto case 4;
            case 6:
                explanations[5] = "기본 데미지 <color=#FF00C7>15</color> 증가";
                goto case 5;
            case 7:
                explanations[6] = "떨어지는 코어 <color=#FF00C7>1</color>개 추가";
                goto case 6;
            case 8:
                explanations[7] = "기본 데미지 <color=#FF00C7>20</color> 증가";
                goto case 7;
            case 9:
                explanations[8] = "떨어지는 코어 <color=#FF00C7>1</color>개 추가, 기본 데미지 <color=#FF00C7>30</color> 증가";
                goto case 8;
        }
    }

    private IEnumerator Dump()
    {
        GameObject proj;
        for (int i = 0; i < finalWeaponData.multiProjectile; i++)
        {
            Vector3 fallingDirection = fallingDirections[Random.Range(0, fallingDirections.Length)];
            GameObject randomTargetVirus = MonsterScanner.ScanRandomObject(transform.position, dumpAreaRadius, LayerMask.GetMask("Virus"));
            Vector3 finalPosition;
            if (randomTargetVirus != null)
            {
                finalPosition = randomTargetVirus.transform.position - fallingDirection * dumpStartHeight;
            }
            else
            {
                finalPosition = GetRandomPosition(fallingDirection);
            }
            proj = PoolManager.instance.GetObject(projectilePool, finalPosition, Quaternion.identity);
            proj.GetComponent<P_CoreDump>().Initialize(finalWeaponData, fallingDirection);
            shootSFX?.Play();
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 현재는 원 범위로 구현되어 있음, 직사각형 범위로 변경할 수도 있음
    private Vector3 GetRandomPosition(Vector3 fallingDirection)
    {
        Vector2 randPoint = Random.insideUnitCircle * dumpAreaRadius;
        return transform.position + new Vector3(randPoint.x, 0, randPoint.y) - fallingDirection * dumpStartHeight;
    }
}
