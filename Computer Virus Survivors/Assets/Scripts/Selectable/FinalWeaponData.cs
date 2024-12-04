using System;
using UnityEngine;

public class FinalWeaponData
{
    public string weaponName;   // 무기 이름
    public int stat_totalDamage;     // 누적 데미지
    public int stat_killcount;  // 킬 카운트

    public int damage; // 최종 공격력
    public int multiProjectile; // 최종 다중 발사체 수
    public float attackPeriod;  // 최종 공격 주기
    public float attackRange;   // 최종 공격 범위
    public int critPoint;       // 최종 치명타 공격 대미지 배율
    public int critProbability;  // 최종 치명타 공격 확률
    public float knockbackTime; // 넉백 시간

    // 치명타 여부를 판단하고 최종 공격력을 반환
    public int GetFinalDamage()
    {
        return damage * (IsCritical() ? critPoint : 100) / 100;
    }

    public DamageData GetDamageData()
    {
        return new DamageData(this);
    }

    private bool IsCritical()
    {
        return UnityEngine.Random.Range(0, 100) < critProbability;
    }

    public void IncrementKillCount()
    {
        stat_killcount++;
    }

}

public class DamageData
{

    public int finalDamage;
    public float knockbackTime;
    public string weaponName;
    public bool isCritical;
    public Action incrementKillCount;

    public DamageData(FinalWeaponData finalWeaponData)
    {
        this.finalDamage = finalWeaponData.GetFinalDamage();
        this.knockbackTime = finalWeaponData.knockbackTime;
        this.weaponName = finalWeaponData.weaponName;
        finalWeaponData.stat_totalDamage += finalDamage;
        isCritical = finalDamage > finalWeaponData.damage;
        incrementKillCount = finalWeaponData.IncrementKillCount;
    }


}
