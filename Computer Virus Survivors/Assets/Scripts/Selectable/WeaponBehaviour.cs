using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

abstract public class WeaponBehaviour : SelectableBehaviour, IPlayerStatObserver
{

    /// <summary>
    /// 초기값 설정
    /// </summary>

    [Header("발사체 풀")]
    [SerializeField]
    private PoolType projectilePoolType;

    [Header("기본 공격력(int)")]
    [SerializeField]
    private int initialDamage;   // 기본 공격력

    [Header("기본 발사체 개수(int, 개)")]
    [SerializeField]
    private int initialMultiProjectile;  // 기본 다중 발사체 수

    [Header("기본 공격 주기(float, 초)")]
    [Min(0)]
    [SerializeField]
    private float initialAttackPeriod;   // 기본 공격 주기

    [Header("기본 공격 범위(float)")]
    [Min(0)]
    [SerializeField]
    private float initialAttackRange;    // 기본 공격 범위

    [Header("기본 추가 치명타 확률(int, %)")]
    [SerializeField]
    private int initialAddiCritProbability;  // 기본 치명타 공격 확률

    [Header("기본 추가 치명타 대미지(int, %)")]
    [SerializeField]
    private int initialAddiCritPoint;    // 기본 치명타 공격 대미지 배율




    /// <summary>
    /// 런타임 변수
    /// </summary>

    private int basicDamage;
    private int basicMultiProjectile;
    private float basicAttackPeriod;
    private float basicAttackRange;
    private int basicAddiCritProbability;
    private int basicAddiCritPoint;
    protected PlayerStatEventCaller playerStatEventCaller;
    protected PlayerStat playerStat;
    protected Coroutine attackCoroutine;
    protected PoolType projectilePool;

    /// <summary>
    /// 런타임 get; set;
    /// </summary>

    // SelectableBehaviour에서 상속받음
    // public string ObjectName { get; }
    // public int MaxLevel { get; }
    // public int CurrentLevel { get; protected set; }
    // public List<string> Explanations { get { return explanations; } }
    protected int BasicDamage
    {
        get
        {
            return basicDamage;
        }
        set
        {
            basicDamage = value;
            CalcAttackDamage();
        }
    }
    protected int BasicMultiProjectile
    {
        get
        {
            return basicMultiProjectile;
        }
        set
        {
            if (value < 1)
            {
                value = 1;
            }
            basicMultiProjectile = value;
            CalcMultiProjectile();
        }
    }
    protected float BasicAttackPeriod
    {
        get
        {
            return basicAttackPeriod;
        }
        set
        {
            if (value < 0.02f)
            {
                value = 0.02f;
            }
            basicAttackPeriod = value;
            CalcAttackPeriod();
        }
    }
    protected float BasicAttackRange
    {
        get
        {
            return basicAttackRange;
        }
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            basicAttackRange = value;
            CalcAttackRange();
        }
    }
    protected int BasicAdditionalCritProbability
    {
        get
        {
            return basicAddiCritProbability;
        }
        set
        {
            basicAddiCritProbability = value;
            CalcCritProbability();
        }
    }
    protected int BasicAdditionalCritPoint
    {
        get
        {
            return basicAddiCritPoint;
        }
        set
        {
            basicAddiCritPoint = value;
            CalcCritPoint();
        }
    }


    /// <summary>
    /// 최종 값
    /// </summary>

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
    protected abstract IEnumerator Attack();


    /// <summary>
    /// PlayerStat받아서 저장
    /// PlayerStatEventCaller받아서 저장
    /// final 값들 초기화
    /// </summary>
    /// <param name="playerStat"></param>
    /// <param name="caller"></param>
    public override void Initialize()
    {
        playerStat = Player.GetComponent<PlayerController>().playerStat;
        playerStatEventCaller = Player.GetComponent<PlayerController>().statEventCaller;
        playerStatEventCaller.StatChanged += OnStatChanged;

        projectilePool = projectilePoolType;
        BasicDamage = initialDamage;
        BasicMultiProjectile = initialMultiProjectile;
        BasicAttackPeriod = initialAttackPeriod;
        BasicAttackRange = initialAttackRange;
        BasicAdditionalCritProbability = initialAddiCritProbability;
        BasicAdditionalCritPoint = initialAddiCritPoint;

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
        finalMultiProjectile = BasicMultiProjectile + playerStat.MultiProjectile;
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
