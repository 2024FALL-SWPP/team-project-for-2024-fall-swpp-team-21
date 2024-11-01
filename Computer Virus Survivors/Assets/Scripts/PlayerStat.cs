using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerStat
{

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
        }
    }

    public int CritAttackProbability
    {
        get
        {
            return critAttackProbability;
        }
        set
        {
            critAttackProbability = value;
        }
    }

    public int CritAttackPoint
    {
        get
        {
            return critAttackPoint;
        }
        set
        {
            critAttackPoint = value;
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
    private int critAttackProbability;      // 치명타 확률
    private int critAttackPoint;            // 치명타시 대미지 배율 (100 + critAttackPoint)%

    private int expGainRatio;               // 경험치 획득 비율 (100 + expGainRatio)%
    private int playerLevel;                // 플레이어 레벨
    private int currentExp;                 // 현재 경험치
    private float expGainRange;             // 경험치 획득 범위
    private float moveSpeed;                // 이동 속도

    private List<int> maxExpList;           // 최대 경험치 리스트
    private List<WeaponBehaviour> weapons;  // 무기 리스트
    // TODO : "ItemBehavior" 아이템 구현
    //private List<ItemBehaviour> items;    // 아이템 리스트

    public PlayerStat()
    {
        MaxHp = GameConstants.DefaultMaxHp;
        CurrentHP = GameConstants.DefaultCurrentHp;
        HealthRezenPer10 = GameConstants.DefaultHealthRegenPer10;
        DefencePoint = GameConstants.DefaultDefencePoint;
        EvadeProbability = GameConstants.DefaultEvadeProbability;
        InvincibleFrame = GameConstants.DefaultInvincibleFrame;

        AttackPoint = GameConstants.DefaultAttackPoint;
        MultiProjectile = GameConstants.DefaultMultiProjectile;
        AttackSpeed = GameConstants.DefaultAttackSpeed;
        AttackRange = GameConstants.DefaultAttackRange;
        CritAttackProbability = GameConstants.DefaultCritAttackProbability;
        CritAttackPoint = GameConstants.DefaultCritAttackPoint;

        ExpGainRatio = GameConstants.DefaultExpGainRatio;
        PlayerLevel = GameConstants.DefaultPlayerLevel;
        CurrentExp = GameConstants.DefaultCurrentExp;
        ExpGainRange = GameConstants.DefaultExpGainRange;
        MoveSpeed = GameConstants.DefaultMoveSpeed;

        maxExpList = GameConstants.ExpList;
        weapons = new List<WeaponBehaviour>();
        // TODO : items 초기화
    }

    public void GetWeapon(WeaponBehaviour weapon)
    {
        weapons.Add(weapon);
        // TODO : 무기 효과 적용
    }

    // public void GetItem(ItemBehaviour item)
    // {
    //     // TODO : 아이템 효과 적용
    // }

}
