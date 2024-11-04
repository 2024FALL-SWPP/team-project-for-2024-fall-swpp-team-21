using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Player의 데이터를 저장하는 ScriptableObject
[CreateAssetMenu(fileName = "PlayerStatData", menuName = "ScriptableObj/PlayerStatData", order = 0)]
public class PlayerStatData : ScriptableObject
{

    public string characterName;           // 캐릭터 이름

    public int maxHP;                      // 최대 체력
    public int currentHP;                  // 현재 체력
    public int healthRezenPer10;           // 10초마다 체력 회복량
    public int defencePoint;               // 방어 포인트 (50 / (50 + defencePoint))%
    public int evadeProbability;           // 회피 확률
    public int invincibleFrame;            // 무적 프레임

    public int attackPoint;                // 공격력 배율 (100 + attackPoint)%
    public int multiProjectile;            // 다중 발사체 수
    public int attackSpeed;                // 공격 속도   attackPeriod = 100 / (attackSpeed) : default = 100
    public int attackRange;                // 공격 범위
    public int critProbability;      // 치명타 확률
    public int critPoint;            // 치명타시 대미지 배율 (100 + critAttackPoint)%

    public int expGainRatio;               // 경험치 획득 비율 (100 + expGainRatio)%
    public int playerLevel;                // 플레이어 레벨
    public int currentExp;                 // 현재 경험치
    public float expGainRange;             // 경험치 획득 범위
    public float moveSpeed;                // 이동 속도

    public readonly int[] maxExpList =
    {
        100, 300, 500, 700, 900,
        1500, 2000, 2500, 3000, 3500,
        4500, 5500, 6500, 7500, 8500,
        10000, 12000, 14000, 16000, 18000,
        20000, 25000, 30000, 35000, 40000,
        50000, 60000, 70000, 80000, 90000,
        100000, 120000, 140000, 160000, 180000,
        200000,
    };
}



