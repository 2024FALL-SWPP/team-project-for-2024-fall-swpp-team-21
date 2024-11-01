using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

abstract public class WeaponBehaviour : SelectableBehaviour, IPlayerStatObserver
{

    [SerializeField] protected ProjectileBehaviour projectile;  // 발사체 프리팹
    [SerializeField] private int basicDamage;   // 기본 공격력
    [SerializeField] private int basicMultiProjectile;  // 기본 다중 발사체 수
    [SerializeField] private float basicAttackPeriod;   // 기본 공격 주기
    [SerializeField] private float basicAttackRange;    // 기본 공격 범위
    [SerializeField] private int basicAdditionalCritProbability;  // 기본 치명타 공격 확률
    [SerializeField] private int basicAdditionalCritPoint;    // 기본 치명타 공격 대미지 배율


    protected PlayerStatEventCaller playerStatEventCaller;  // PlayerStatEventCaller
    protected Coroutine attackCoroutine;
    protected int finalDamage;  // 최종 공격력
    protected int finalMultiProjectile; // 최종 다중 발사체 수
    protected float finalAttackPeriod;  // 최종 공격 주기
    protected float finalAttackRange;   // 최종 공격 범위
    protected int finalCritPoint;       // 최종 치명타 공격 대미지 배율

    private int finalCritProbability;  // 최종 치명타 공격 확률


    /// <summary>
    /// 무��별로 서로 다른 공격을 구현하기 위한 추상 메소드
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Attack();


    /// <summary>
    /// PlayerStat으로부터 무기 스탯 초기화.
    /// PlayerStatEventCaller받아서 저장
    /// </summary>
    /// <param name="playerStat"></param>
    /// <param name="caller"></param>
    public void InitializeWeapon(PlayerStat playerStat, PlayerStatEventCaller caller)
    {
        CalcAttackDamage(playerStat.AttackPoint);
        CalcAttackPeriod(playerStat.AttackSpeed);
        CalcAttackRange(playerStat.AttackRange);
        CalcMultiProjectile(playerStat.MultiProjectile);
        CalcCritProbability(playerStat.CritProbability);
        CalcCritPoint(playerStat.CritPoint);
        playerStatEventCaller = caller;
        playerStatEventCaller.StatChanged += OnStatChanged;
        StartAttack();
    }


    /// <summary>
    /// 공격을 시작함
    /// </summary>
    public void StartAttack()
    {
        attackCoroutine = StartCoroutine(Attack());
    }


    /// <summary>
    /// 공격을 중지함
    /// </summary>
    public void StopAttack()
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
    private void CalcAttackDamage(int attackPoint)
    {
        finalDamage = basicDamage * attackPoint / 100;
    }


    /// <summary>
    /// 공격 주기를 계산함
    /// </summary>
    /// <param name="attackSpeed"></param>
    private void CalcAttackPeriod(int attackSpeed)
    {
        finalAttackPeriod = basicAttackPeriod / attackSpeed * 100;
    }


    /// <summary>
    /// 공격 범위를 계산함
    /// </summary>
    /// <param="attackRange"></param>
    private void CalcAttackRange(float attackRange)
    {
        finalAttackRange = basicAttackRange * attackRange / 100;
    }


    /// <summary>
    /// 다중 발사체의 수를 계산함
    /// </summary>
    /// <param name="multiProjectile"></param>
    private void CalcMultiProjectile(int multiProjectile)
    {
        finalMultiProjectile = basicMultiProjectile * multiProjectile;
    }


    /// <summary>
    /// 치명타 공격 확률을 계산함
    /// </summary>
    /// <param name="critAttackProbability"></param>
    private void CalcCritProbability(int critAttackProbability)
    {
        finalCritProbability = basicAdditionalCritProbability + critAttackProbability;
    }


    /// <summary>
    /// 치명타 공격 시 대미지 배율을 계산함
    /// </summary>
    /// <param name="critAttackPoint"></param>
    private void CalcCritPoint(int critAttackPoint)
    {
        finalCritPoint = basicAdditionalCritPoint + critAttackPoint;
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
                CalcAttackDamage((int) e.NewValue);
                break;
            case nameof(PlayerStat.AttackSpeed):
                CalcAttackPeriod((int) e.NewValue);
                break;
            case nameof(PlayerStat.AttackRange):
                CalcAttackRange((float) e.NewValue);
                break;
            case nameof(PlayerStat.MultiProjectile):
                CalcMultiProjectile((int) e.NewValue);
                break;
            case nameof(PlayerStat.CritProbability):
                CalcCritProbability((int) e.NewValue);
                break;
            case nameof(PlayerStat.CritPoint):
                CalcCritPoint((int) e.NewValue);
                break;
        }
    }

}
