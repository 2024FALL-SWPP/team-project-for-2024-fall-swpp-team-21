using UnityEngine;

public class FinalWeaponData
{
    public int damage; // 최종 공격력
    public int multiProjectile; // 최종 다중 발사체 수
    public float attackPeriod;  // 최종 공격 주기
    public float attackRange;   // 최종 공격 범위
    public int critPoint;       // 최종 치명타 공격 대미지 배율
    public int critProbability;  // 최종 치명타 공격 확률

    // 치명타 여부를 판단하고 최종 공격력을 반환
    public int GetFinalDamage()
    {
        return damage * (IsCritical() ? critPoint : 100) / 100;
    }

    private bool IsCritical()
    {
        return Random.Range(0, 100) < critProbability;
    }

}