using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
public class PlayerStat : IPlayerStatObserver
{

    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

    private PlayerStatEventCaller statEventCaller;

    public int MaxHP
    {
        get
        {
            return maxHP;
        }
        set
        {

            int increasedHp = value - maxHP;
            maxHP = value;
            if (currentHP + increasedHp < 0)
            {
                CurrentHP = 1;
            }
            else
            {
                CurrentHP += increasedHp;
            }
            statEventCaller.Invoke(nameof(MaxHP), maxHP);

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
            int originalHP = currentHP;
            currentHP = value;
            if (currentHP < 0)
            {
                currentHP = 0;
            }
            else if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
            if (originalHP != currentHP)
            {
                statEventCaller.Invoke(nameof(CurrentHP), currentHP);

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
            statEventCaller.Invoke(nameof(HealthRezenPer10), value);
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
            statEventCaller.Invoke(nameof(DefencePoint), value);
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
            statEventCaller.Invoke(nameof(EvadeProbability), value);
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
            statEventCaller.Invoke(nameof(InvincibleFrame), value);
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
            statEventCaller.Invoke(nameof(AttackPoint), value);
        }
    }

    public int MultiProjectile
    {
        get
        {
            return multiProjectile;
        }
        set
        {
            multiProjectile = value;
            statEventCaller.Invoke(nameof(MultiProjectile), value);
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
            statEventCaller.Invoke(nameof(AttackSpeed), value);
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
            statEventCaller.Invoke(nameof(AttackRange), value);
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
            statEventCaller.Invoke(nameof(CritProbability), value);
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
            statEventCaller.Invoke(nameof(CritPoint), value);
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
            statEventCaller.Invoke(nameof(ExpGainRatio), value);
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
            statEventCaller.Invoke(nameof(PlayerLevel), value);
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
            statEventCaller.Invoke(nameof(CurrentExp), value);
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
            statEventCaller.Invoke(nameof(ExpGainRange), value);
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
            statEventCaller.Invoke(nameof(MoveSpeed), value);
        }
    }

    public int MaxExp
    {
        get
        {
            return maxExp;
        }
        private set
        {
            maxExp = value;
            statEventCaller.Invoke(nameof(MaxExp), value);
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
    private int maxExp;

    private int[] maxExpList;           // 최대 경험치 리스트
    private List<WeaponBehaviour> weapons;  // 무기 리스트
    private List<ItemBehaviour> items;    // 아이템 리스트

    public void Initialize(PlayerStatData playerStatData, PlayerStatEventCaller eventCaller)
    {
        statEventCaller = eventCaller;
        statEventCaller.StatChangedHandler += OnStatChanged;

        MaxHP = playerStatData.maxHP;
        CurrentHP = playerStatData.currentHP;
        HealthRezenPer10 = playerStatData.healthRezenPer10;
        DefencePoint = playerStatData.defencePoint;
        EvadeProbability = playerStatData.evadeProbability;
        InvincibleFrame = playerStatData.invincibleFrame;

        AttackPoint = playerStatData.attackPoint;
        MultiProjectile = playerStatData.multiProjectile;
        AttackSpeed = playerStatData.attackSpeed;
        AttackRange = playerStatData.attackRange;
        CritProbability = playerStatData.critProbability;
        CritPoint = playerStatData.critPoint;

        ExpGainRatio = playerStatData.expGainRatio;
        PlayerLevel = playerStatData.playerLevel;
        ExpGainRange = playerStatData.expGainRange;
        MoveSpeed = playerStatData.moveSpeed;

        weapons = new List<WeaponBehaviour>();
        items = new List<ItemBehaviour>();

        maxExpList = playerStatData.maxExpList;
        MaxExp = maxExpList[playerLevel];
        CurrentExp = playerStatData.currentExp;
    }


    public List<SelectableBehaviour> GetPlayerWeaponInfos()
    {
        return weapons.Cast<SelectableBehaviour>().ToList();
    }

    public List<SelectableBehaviour> GetPlayerItemInfos()
    {
        return items.Cast<SelectableBehaviour>().ToList();
    }

    public void TakeSelectable(SelectableBehaviour selectable)
    {
        // level up
        selectable.Acquire();

        // 신규 무기나 아이템이면 추가
        if (selectable is WeaponBehaviour && !weapons.Contains(selectable as WeaponBehaviour))
        {
            Debug.Log("무기 추가 : " + selectable.ObjectName);
            weapons.Add(selectable as WeaponBehaviour);
        }
        else if (selectable is ItemBehaviour && !items.Contains(selectable as ItemBehaviour))
        {
            Debug.Log("아이템 추가 : " + selectable.ObjectName);
            items.Add(selectable as ItemBehaviour);
        }
    }


    public void OnStatChanged(object sender, StatChangedEventArgs e)
    {
        if (e.StatName == nameof(CurrentExp))
        {
            CurrentExpChanged();
        }
    }

    // 아이템 선택창, 경험치 GUI 구현의 디테일을 위해 스탯이 변하는 순서가 중요함
    // 경험치가 maxExp보다 커지면
    // -> 경험치 바가 100%가 됨
    // -> PlayerLevel이 1 증가함 -> 아이템 선택창이 뜸
    // -> 현재 경험치가 maxExp만큼 감소함 -> 경험치 바가 n%가 됨
    // -> maxExp가 PlayerLevel에 맞게 설정됨 -> 경험치 바가 정상화 됨
    // 하드 코딩이고, 변수의 첫글자가 소문자인지 대문자인지에 따라 양상이 달라지기 때문에 버그가 일어날 가능성 다분함
    private async void CurrentExpChanged()
    {
        await semaphore.WaitAsync();
        if (currentExp >= maxExp)
        {
            PlayerLevel++; // Invoke Player Level Changed Event -> Show Item Selection Canvas

            await WaitForItemSelection();
            currentExp -= maxExp;
            MaxExp = maxExpList[Mathf.Min(PlayerLevel, maxExpList.Length - 1)];
            statEventCaller.Invoke(nameof(CurrentExp), currentExp); // 재귀 호출이 일어날 수 있음
        }
        semaphore.Release();
    }

    private Task WaitForItemSelection()
    {
        var tcs = new TaskCompletionSource<bool>();

        void handler(SelectableBehaviour selectable)
        {
            TakeSelectable(selectable);
            tcs.SetResult(true);
            ItemSelectCanvasManager.instance.SelectionHandler -= handler;
        }

        ItemSelectCanvasManager.instance.SelectionHandler += handler;

        return tcs.Task;
    }
}
