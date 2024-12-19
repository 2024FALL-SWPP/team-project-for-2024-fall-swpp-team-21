using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : Singleton<PauseCanvas>, IState, IPlayerStatObserver
{

    public event Action ResumeHandler;

    [SerializeField] private PlayerStatEventCaller playerStatEventCaller;
    [SerializeField] private GameObject baseSpecObject;
    [SerializeField] private GameObject weaponStatisticsPanel;
    [SerializeField] private CanvasSoundPreset canvasSoundPreset;
    private Dictionary<string, SingleSpec> singleSpecs;

    private GameObject weaponSingleStat;
    private bool initialized = false;
    public override void Initialize()
    {
        InitializeSingleSpecs();
        playerStatEventCaller.StatChangedHandler += OnStatChanged;
        weaponSingleStat = weaponStatisticsPanel.transform.GetChild(0).gameObject;
        gameObject.SetActive(false);
    }

    public void OnResumeClicked()
    {
        ResumeHandler?.Invoke();
    }

    public void OnGotoMainClicked()
    {
        GameManager.instance.GotoMainScene();
    }

    public void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnEnter()
    {
        UpdateStatistics();
        transform.SetAsLastSibling();

        UISoundManager.instance.PlaySound(canvasSoundPreset.EnterSound);
    }

    public void OnExit()
    {
        UISoundManager.instance.PlaySound(canvasSoundPreset.ExitSound);
    }

    public void OnStatChanged(object sender, StatChangedEventArgs e)
    {
        if (singleSpecs.ContainsKey(e.StatName))
        {
            if (e.NewValue is int v)
            {
                singleSpecs[e.StatName].UpdateValue(v);
            }
            else if (e.NewValue is float f)
            {
                singleSpecs[e.StatName].UpdateValue((int) f);
            }
        }

    }

    private void InitializeStatistics()
    {


        List<WeaponStatistic> weaponDatas = GameManager.instance.GetWeaponStatistics();

        for (int i = 0; i < weaponDatas.Count; i++)
        {
            GameObject weaponStat = Instantiate(weaponSingleStat, weaponStatisticsPanel.transform);
            TextMeshProUGUI weaponName = weaponStat.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI weaponKillCount = weaponStat.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI weaponDamage = weaponStat.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            weaponName.text = weaponDatas[i].weaponName;
            weaponKillCount.text = weaponDatas[i].killCount.ToString();
            weaponDamage.text = weaponDatas[i].totalDamage.ToString();

        }

    }

    private void UpdateStatistics()
    {

        if (!initialized)
        {
            initialized = true;
            InitializeStatistics();
            return;
        }

        List<WeaponStatistic> weaponDatas = GameManager.instance.GetWeaponStatistics();

        for (int i = 0; i < weaponDatas.Count; i++)
        {
            // 첫번째는 항목명이므로 1부터 시작
            GameObject weaponStat = weaponStatisticsPanel.transform.GetChild(i + 1).gameObject;
            TextMeshProUGUI weaponName = weaponStat.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI weaponKillCount = weaponStat.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI weaponDamage = weaponStat.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            weaponName.text = weaponDatas[i].weaponName;
            weaponKillCount.text = weaponDatas[i].killCount.ToString();
            weaponDamage.text = weaponDatas[i].totalDamage.ToString();

        }
    }

    private void InitializeSingleSpecs()
    {

        Dictionary<string, string> specLabel = new()
        {
            { nameof(PlayerStat.PlayerLevel), "플레이어 레벨" },
            { nameof(PlayerStat.MaxHP), "최대 체력" },
            { nameof(PlayerStat.HealthRezenPer10), "체력 회복량" },
            { nameof(PlayerStat.DefencePoint), "방어력" },
            { nameof(PlayerStat.EvadeProbability), "회피 확률" },
            { nameof(PlayerStat.AttackPoint), "공격력" },
            { nameof(PlayerStat.MultiProjectile), "다중 발사체" },
            { nameof(PlayerStat.AttackSpeed), "공격 속도" },
            { nameof(PlayerStat.AttackRange), "공격 범위" },
            { nameof(PlayerStat.CritProbability), "치명타 확률" },
            { nameof(PlayerStat.CritPoint), "치명타 공격력" },
            { nameof(PlayerStat.ExpGainRatio), "경험치 획득량" },
            { nameof(PlayerStat.ExpGainRange), "경험치 획득 범위" },
            { nameof(PlayerStat.MoveSpeed), "이동 속도" }
        };

        Dictionary<string, string> specUnit = new()
        {
            { nameof(PlayerStat.PlayerLevel), "" },
            { nameof(PlayerStat.MaxHP), "" },
            { nameof(PlayerStat.HealthRezenPer10), "/10초" },
            { nameof(PlayerStat.DefencePoint), "" },
            { nameof(PlayerStat.EvadeProbability), "%" },
            { nameof(PlayerStat.AttackPoint), "%" },
            { nameof(PlayerStat.MultiProjectile), "" },
            { nameof(PlayerStat.AttackSpeed), "%" },
            { nameof(PlayerStat.AttackRange), "%" },
            { nameof(PlayerStat.CritProbability), "%" },
            { nameof(PlayerStat.CritPoint), "%" },
            { nameof(PlayerStat.ExpGainRatio), "%" },
            { nameof(PlayerStat.ExpGainRange), "m" },
            { nameof(PlayerStat.MoveSpeed), "m/s" }
        };

        singleSpecs = new Dictionary<string, SingleSpec>();

        foreach (var spec in specLabel)
        {
            GameObject baseObject = Instantiate(baseSpecObject, baseSpecObject.transform.parent);
            SingleSpec singleSpec = new SingleSpec(baseObject, spec.Key, spec.Value, 0, specUnit[spec.Key]);
            singleSpecs.Add(spec.Key, singleSpec);
        }

        baseSpecObject.SetActive(false);
    }

    private class SingleSpec
    {
        public string key;

        private readonly TextMeshProUGUI labelField;
        private readonly TextMeshProUGUI valueField;
        private readonly TextMeshProUGUI unitField;
        public string label;
        public int value;
        public string unit;

        public SingleSpec(GameObject gameObject, string key, string label, int value, string unit = "")
        {
            labelField = gameObject.transform.Find("Label").GetComponent<TextMeshProUGUI>();
            valueField = gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>();
            unitField = gameObject.transform.Find("Unit").GetComponent<TextMeshProUGUI>();
            this.key = key;
            this.label = label;
            this.value = value;
            this.unit = unit;
            labelField.text = label;
            valueField.text = value.ToString();
            unitField.text = unit;
        }

        public void UpdateValue(int value)
        {
            this.value = value;
            valueField.text = value.ToString();
        }
    }

}
