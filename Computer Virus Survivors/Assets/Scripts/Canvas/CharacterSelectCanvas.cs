using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelectCanvas : Singleton<CharacterSelectCanvas>, IState
{

    [SerializeField] private List<PlayerController> players;
    [SerializeField] private GameObject playerTrailCircle;
    [SerializeField] private GameObject baseSpecObject;

    public event Action GotoPreviousHandler;
    public event Action<PlayerController> CharacterSelectedHandler;

    private int selectedPlayerIndex;
    private Dictionary<string, SingleSpec> singleSpecs;

    /// <summary>
    /// 초기화
    /// </summary>
    public override void Initialize()
    {
        selectedPlayerIndex = 0;
        InitializeSingleSpecs();
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 캐릭터 선택 완료
    /// 스테이지 선택창으로 이동
    /// </summary>
    public void OnSelectClicked()
    {
        CharacterSelectedHandler?.Invoke(players[selectedPlayerIndex]);
    }


    /// <summary>
    /// 다음 플레이어 선택
    /// </summary>
    public void OnRightPlayerClicked()
    {
        selectedPlayerIndex = (selectedPlayerIndex + 1 + players.Count) % players.Count;
        UpdatePlayerInfo();
    }


    /// <summary>
    /// 이전 플레이어 선택
    /// </summary>
    public void OnLeftPlayerClicked()
    {
        selectedPlayerIndex = (selectedPlayerIndex - 1 + players.Count) % players.Count;
        UpdatePlayerInfo();
    }


    /// <summary>
    /// 이전 화면으로 이동(메인화면)
    /// </summary>
    public void OnGotoPreviousClicked()
    {
        GotoPreviousHandler?.Invoke();
    }


    /// <summary>
    /// 왼쪽 스탯 패널에 선택된 캐릭터의 기본 스펙 표시
    /// </summary>
    private void UpdatePlayerInfo()
    {
        PlayerStatData currentPlayerStatData = players[selectedPlayerIndex].playerStatData;

        // 플레이어 정보 업데이트

    }


    /// <summary>
    /// 캐릭터 선택 화면으로 이동되었을 때 실행
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnEnter()
    {
        // 카메라를 캐릭터 선택 룸으로 이동
        throw new System.NotImplementedException();
    }


    /// <summary>
    /// 캐릭터 선택 화면에서 나갈 때 실행
    /// </summary>
    public void OnExit()
    {
        // 카메라를 원래 위치로 이동
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 캐릭터 스펙 정보 초기화
    /// </summary>
    private void InitializeSingleSpecs()
    {

        Dictionary<string, string> specLabel = new()
        {
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


    /// <summary>
    /// 캐릭터 스펙 정보 표현을 위한 클래스
    /// </summary>
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
