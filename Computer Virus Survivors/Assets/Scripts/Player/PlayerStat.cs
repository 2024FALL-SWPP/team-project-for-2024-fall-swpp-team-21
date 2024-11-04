using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat
{

    private PlayerStatEventCaller statEventCaller;

    public int MaxHp
    {
        get
        {
            return maxHP;
        }
        set
        {
            if (value > 0)
            {
                maxHP = value;
                statEventCaller.OnStatChanged(nameof(MaxHp), value);
            }
        }
    }

    public int CurrentHP
    {
        get
        {
            return currentHP;
        }
        set
        {
            currentHP = value;
            if (currentHP < 0)
            {
                currentHP = 0;
            }
            statEventCaller.OnStatChanged(nameof(CurrentHP), value);
        }
    }

    public int HealthRezenPer10
    {
        /// 음수면 10초마다 체력이 깎임
        get
        {
            return healthRezenPer10;
        }
        set
        {
            healthRezenPer10 = value;
            statEventCaller.OnStatChanged(nameof(HealthRezenPer10), value);
        }
    }

    public int DefencePoint
    {
        get
        {
            return defencePoint;
        }
        set
        {
            defencePoint = value;
            statEventCaller.OnStatChanged(nameof(DefencePoint), value);
        }
    }

    public int EvadeProbability
    {
        /// 음수면 회피 확률이 0이지만, 다시 양수로 만들기 위해 더 많은 스탯을 먹도록 음수도 허용
        get
        {
            return evadeProbability;
        }
        set
        {
            evadeProbability = value;
            statEventCaller.OnStatChanged(nameof(EvadeProbability), value);
        }
    }

    public int InvincibleFrame
    {
        /// 무적 주기는 변경할 일이 없을듯
        get
        {
            return invincibleFrame;
        }
        set
        {
            invincibleFrame = value;
            statEventCaller.OnStatChanged(nameof(InvincibleFrame), value);
        }
    }

    public int AttackPoint
    {
        get
        {
            return attackPoint;
        }
        set
        {
            attackPoint = value;
            statEventCaller.OnStatChanged(nameof(AttackPoint), value);
        }
    }

    public int MultiProjectile
    {
        /// 최소 1의 값을 가져야 함.
        get
        {
            return multiProjectile;
        }
        set
        {
            multiProjectile = value;
            statEventCaller.OnStatChanged(nameof(MultiProjectile), value);
        }
    }

    public int AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
        set
        {
            attackSpeed = value;
            statEventCaller.OnStatChanged(nameof(AttackSpeed), value);
        }
    }

    public int AttackRange
    {
        get
        {
            return attackRange;
        }
        set
        {
            attackRange = value;
            statEventCaller.OnStatChanged(nameof(AttackRange), value);
        }
    }

    public int CritProbability
    {
        get
        {
            return critProbability;
        }
        set
        {
            critProbability = value;
            statEventCaller.OnStatChanged(nameof(CritProbability), value);
        }
    }

    public int CritPoint
    {
        get
        {
            return critPoint;
        }
        set
        {
            critPoint = value;
            statEventCaller.OnStatChanged(nameof(CritPoint), value);
        }
    }

    public int ExpGainRatio
    {
        get
        {
            return expGainRatio;
        }
        set
        {
            expGainRatio = value;
            statEventCaller.OnStatChanged(nameof(ExpGainRatio), value);
        }
    }

    public int PlayerLevel
    {
        get
        {
            return playerLevel;
        }
        set
        {
            playerLevel = value;
            statEventCaller.OnStatChanged(nameof(PlayerLevel), value);
        }
    }

    public int CurrentExp
    {
        get
        {
            return currentExp;
        }
        set
        {
            currentExp = value;
            statEventCaller.OnStatChanged(nameof(CurrentExp), value);
        }
    }

    public float ExpGainRange
    {
        get
        {
            return expGainRange;
        }
        set
        {
            expGainRange = value;
            statEventCaller.OnStatChanged(nameof(ExpGainRange), value);
        }
    }

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
            statEventCaller.OnStatChanged(nameof(MoveSpeed), value);
        }
    }

    private int maxHP;                      // 최대 체력
    private int currentHP;                  // 현재 체력
    private int healthRezenPer10;           // 10초마다 체력 회복량
    private int defencePoint;               // 방어 포인트 (50 / (50 + defencePoint))%
    private int evadeProbability;           // 회피 확률
    private int invincibleFrame;            // 무적 프레임

    private int attackPoint;                // 공격력 배율 (100 + attackPoint)%
    private int multiProjectile;            // 다중 발사체 수
    private int attackSpeed;                // 공격 속도   attackPeriod = 100 / (attackSpeed) : default = 100
    private int attackRange;                // 공격 범위
    private int critProbability;            // 치명타 확률
    private int critPoint;                  // 치명타시 대미지 배율 (100 + critPoint)%

    private int expGainRatio;               // 경험치 획득 비율 (100 + expGainRatio)%
    private int playerLevel;                // 플레이어 레벨
    private int currentExp;                 // 현재 경험치
    private float expGainRange;             // 경험치 획득 범위
    private float moveSpeed;                // 이동 속도

    private int[] maxExpList;           // 최대 경험치 리스트
    private List<WeaponBehaviour> weapons;  // 무기 리스트
                                            // TODO : "ItemBehavior" 아이템 구현
                                            //private List<ItemBehaviour> items;    // 아이템 리스트

    public void Initialize(PlayerStatData playerStatData, PlayerStatEventCaller eventCaller)
    {
        statEventCaller = eventCaller;

        maxHP = playerStatData.maxHP;
        currentHP = playerStatData.currentHP;
        healthRezenPer10 = playerStatData.healthRezenPer10;
        defencePoint = playerStatData.defencePoint;
        evadeProbability = playerStatData.evadeProbability;
        invincibleFrame = playerStatData.invincibleFrame;

        attackPoint = playerStatData.attackPoint;
        multiProjectile = playerStatData.multiProjectile;
        attackSpeed = playerStatData.attackSpeed;
        attackRange = playerStatData.attackRange;
        critProbability = playerStatData.critProbability;
        critPoint = playerStatData.critPoint;

        expGainRatio = playerStatData.expGainRatio;
        playerLevel = playerStatData.playerLevel;
        currentExp = playerStatData.currentExp;
        expGainRange = playerStatData.expGainRange;
        moveSpeed = playerStatData.moveSpeed;

        maxExpList = playerStatData.maxExpList;
        weapons = new List<WeaponBehaviour>();
    }


    public void GetExp(int exp)
    {
        currentExp += exp;
        if (currentExp >= maxExpList[playerLevel])
        {
            currentExp -= maxExpList[playerLevel];
            playerLevel++;
            // 이제 다른 코드의 OnStatChanged에서 selectable 띄움
        }
    }
}
