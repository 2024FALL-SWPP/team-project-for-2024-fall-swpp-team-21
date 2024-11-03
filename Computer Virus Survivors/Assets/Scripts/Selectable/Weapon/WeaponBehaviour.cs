using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

abstract public class WeaponBehaviour : SelectableBehaviour, IPlayerStatObserver
{

    // 레벨업에 따른 설명을 저장하기 위해 어쩔 수 없이 만듦
    // 만드는 김에 초기값도 저장함
    // 런타임 데이터도 저장함
    [SerializeField] private WeaponData weaponData;

    // 아래 변수들의 데이터는 weaponData에 저장되어 있음
    // set{}을 정의하기 위해 따로 뺌
    protected int level
    {
        get
        {
            return weaponData.currentLevel;
        }
        set
        {
            weaponData.currentLevel = value;
        }
    }

    protected PoolType projectile;
    protected int BasicDamage
    {
        get
        {
            return weaponData.basicDamage;
        }
        set
        {
            weaponData.basicDamage = value;
            CalcAttackDamage();
        }
    }
    protected int BasicMultiProjectile
    {
        get
        {
            return weaponData.basicMultiProjectile;
        }
        set
        {
            weaponData.basicMultiProjectile = value;
            CalcMultiProjectile();
        }
    }
    protected float BasicAttackPeriod
    {
        get
        {
            return weaponData.basicAttackPeriod;
        }
        set
        {
            weaponData.basicAttackPeriod = value;
            CalcAttackPeriod();
        }
    }
    protected float BasicAttackRange
    {
        get
        {
            return weaponData.basicAttackRange;
        }
        set
        {
            weaponData.basicAttackRange = value;
            CalcAttackRange();
        }
    }
    protected int BasicAdditionalCritProbability
    {
        get
        {
            return weaponData.basicAddiCritProbability;
        }
        set
        {
            weaponData.basicAddiCritProbability = value;
            CalcCritProbability();
        }
    }
    protected int BasicAdditionalCritPoint
    {
        get
        {
            return weaponData.basicAddiCritPoint;
        }
        set
        {
            weaponData.basicAddiCritPoint = value;
            CalcCritPoint();
        }
    }

    protected PlayerStatEventCaller playerStatEventCaller;
    protected PlayerStat playerStat;
    protected Coroutine attackCoroutine;

    protected int finalDamage { get; private set; }  // 최종 공격력
    protected int finalMultiProjectile { get; private set; } // 최종 다중 발사체 수
    protected float finalAttackPeriod { get; private set; }  // 최종 공격 주기
    protected float finalAttackRange { get; private set; }   // 최종 공격 범위
    protected int finalCritPoint { get; private set; }       // 최종 치명타 공격 대미지 배율
    protected int finalCritProbability { get; private set; }  // 최종 치명타 공격 확률


    /// <summary>
    /// 무기별로 서로 다른 공격을 구현하기 위한 추상 메소드
    /// </summary>
    /// <returns></returns>
    abstract protected IEnumerator Attack();


    /// <summary>
    /// 무기별로 레벨업에 따른 효과를 정의하는 추상 메소드
    /// </summary>
    /// <param name="level"></param>
    abstract protected void LevelUpEffect(int level);

    /// <summary>
    /// 레벨업시 호출되는 메소드
    /// 각 무기를 정의하는 클래스에서 정의하지 않도록 봉인함
    /// </summary>
    sealed protected override void LevelUp()
    {
        if (level < weaponData.levelMax)
        {
            level++;
            LevelUpEffect(level);
        }
    }



    /// <summary>
    /// 플레이어가 이 무기를 선택하면 호출됨
    /// </summary>
    /// <param name="player"></param>
    public override void GetSelectable(PlayerController player)
    {
        if (level == 0)
        {
            this.player = player.gameObject;
            InitializeWeapon(player.playerStat, player.statEventCaller);
        }

        LevelUp();
    }


    /// <summary>
    /// PlayerStat받아서 저장
    /// PlayerStatEventCaller받아서 저장
    /// final 값들 초기화
    /// </summary>
    /// <param name="playerStat"></param>
    /// <param name="caller"></param>
    private void InitializeWeapon(PlayerStat playerStat, PlayerStatEventCaller caller)
    {
        this.playerStat = playerStat;
        playerStatEventCaller = caller;
        playerStatEventCaller.StatChanged += OnStatChanged;

        projectile = weaponData.projectile;
        BasicDamage = weaponData.basicDamage;
        BasicMultiProjectile = weaponData.basicMultiProjectile;
        BasicAttackPeriod = weaponData.basicAttackPeriod;
        BasicAttackRange = weaponData.basicAttackRange;
        BasicAdditionalCritProbability = weaponData.basicAddiCritProbability;
        BasicAdditionalCritPoint = weaponData.basicAddiCritPoint;

        StartAttack();
    }


    /// <summary>
    /// 공격을 시작함
    /// </summary>
    protected void StartAttack()
    {
        attackCoroutine = StartCoroutine(Attack());
    }


    /// <summary>
    /// 공격을 중지함
    /// </summary>
    protected void StopAttack()
    {
        StopCoroutine(attackCoroutine);
    }


    /// <summary>
    /// 치명타 공격인지 확인함
    /// </summary>
    /// <returns></returns>
    protected bool IsCritical()
    {
        return Random.Range(0, 100) < finalCritProbability;
    }


    /// <summary>
    /// 공격 대미지를 ���산함
    /// </summary>
    /// <param name="attackPoint"></param>
    protected void CalcAttackDamage()
    {
        finalDamage = BasicDamage * playerStat.AttackPoint / 100;
    }


    /// <summary>
    /// 공격 주기를 계산함
    /// </summary>
    /// <param name="attackSpeed"></param>
    protected void CalcAttackPeriod()
    {
        finalAttackPeriod = BasicAttackPeriod / playerStat.AttackSpeed * 100;
    }


    /// <summary>
    /// 공격 범위를 계산함
    /// </summary>
    /// <param="attackRange"></param>
    protected void CalcAttackRange()
    {
        finalAttackRange = BasicAttackRange * playerStat.AttackRange / 100;
    }


    /// <summary>
    /// 다중 발사체의 수를 계산함
    /// </summary>
    /// <param name="multiProjectile"></param>
    protected void CalcMultiProjectile()
    {
        finalMultiProjectile = BasicMultiProjectile * playerStat.MultiProjectile;
    }


    /// <summary>
    /// 치명타 공격 확률을 계산함
    /// </summary>
    /// <param name="critAttackProbability"></param>
    protected void CalcCritProbability()
    {
        finalCritProbability = BasicAdditionalCritProbability + playerStat.CritProbability;
    }


    /// <summary>
    /// 치명타 공격 시 대미지 배율을 계산함
    /// </summary>
    /// <param name="critAttackPoint"></param>
    protected void CalcCritPoint()
    {
        finalCritPoint = BasicAdditionalCritPoint + playerStat.CritPoint;
    }


    /// <summary>
    /// PlayerStatEventCaller로부터 이벤트를 받아서 스탯을 변경함
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void OnStatChanged(object sender, StatChangedEventArgs e)
    {
        switch (e.StatName)
        {
            case nameof(PlayerStat.AttackPoint):
                CalcAttackDamage();
                break;
            case nameof(PlayerStat.AttackSpeed):
                CalcAttackPeriod();
                break;
            case nameof(PlayerStat.AttackRange):
                CalcAttackRange();
                break;
            case nameof(PlayerStat.MultiProjectile):
                CalcMultiProjectile();
                break;
            case nameof(PlayerStat.CritProbability):
                CalcCritProbability();
                break;
            case nameof(PlayerStat.CritPoint):
                CalcCritPoint();
                break;
        }
    }



}
