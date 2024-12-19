using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectCanvas : CanvasBase
{


    [SerializeField] private GameObject playerTrailCircle;
    [SerializeField] private float playerTrailCircleRadius;
    [SerializeField] private GameObject baseSpecObject;
    [SerializeField] private List<GameObject> players;
    [SerializeField] private List<PlayerStatData> playerStatDatas;
    [SerializeField] private TextMeshProUGUI playerNameField;
    [SerializeField] private Button selectButton;

    public event Action GotoPreviousHandler;
    public event Action<GameObject> CharacterSelectedHandler;

    private int selectedPlayerIndex;
    private Dictionary<string, SingleSpec> singleSpecs;
    private List<Vector3> playerPlaces;

    /// <summary>
    /// 초기화
    /// </summary>
    public override void Initialize()
    {
        selectedPlayerIndex = 0;
        ReplacePlayers();
        InitializeSingleSpecs();
        UpdatePlayerInfo();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            GameObject player = players[i];
            player.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1));

            // 원판 돌리기
            // 생각해보니 이렇게 구현할 필요가 없었음
            float targetAngle = Mathf.Atan2(playerPlaces[i].x, playerPlaces[i].z) * Mathf.Rad2Deg;
            Vector3 currentVector = player.transform.position - playerTrailCircle.transform.position;
            float deltaAngle = Mathf.Deg2Rad * Mathf.LerpAngle(Mathf.Atan2(currentVector.x, currentVector.z) * Mathf.Rad2Deg, targetAngle, Time.deltaTime * 8f);
            player.transform.position = playerTrailCircle.transform.position + new Vector3(playerTrailCircleRadius * Mathf.Sin(deltaAngle), 0, playerTrailCircleRadius * Mathf.Cos(deltaAngle));
        }
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
        ReplacePlayers();
        UpdatePlayerInfo();
    }


    /// <summary>
    /// 이전 플레이어 선택
    /// </summary>
    public void OnLeftPlayerClicked()
    {
        selectedPlayerIndex = (selectedPlayerIndex - 1 + players.Count) % players.Count;
        ReplacePlayers();
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
    /// 플레이어 회전초밥 만들기
    /// </summary>
    private void ReplacePlayers()
    {
        if (playerPlaces == null)
        {
            playerPlaces = new List<Vector3>();
            for (int i = 0; i < players.Count; i++)
            {
                float angle = 360f / players.Count;
                playerPlaces.Add(new Vector3(playerTrailCircleRadius * Mathf.Sin(Mathf.Deg2Rad * angle * i), 0, playerTrailCircleRadius * Mathf.Cos(Mathf.Deg2Rad * angle * i)));
            }
        }

        for (int i = 0; i < players.Count; i++)
        {
            int j = (selectedPlayerIndex + i) % players.Count;
            float angle = 360f / players.Count;
            playerPlaces[i] = new Vector3(playerTrailCircleRadius * Mathf.Sin(Mathf.Deg2Rad * angle * j), 0, playerTrailCircleRadius * Mathf.Cos(Mathf.Deg2Rad * angle * j));
        }
    }


    /// <summary>
    /// 왼쪽 스탯 패널에 선택된 캐릭터의 기본 스펙 표시
    /// </summary>
    private void UpdatePlayerInfo()
    {
        PlayerStatData currentPlayerStatData = playerStatDatas[selectedPlayerIndex];

        playerNameField.text = currentPlayerStatData.characterName;
        selectButton.interactable = !currentPlayerStatData.characterName.Equals("???");

        if (currentPlayerStatData.characterName.Equals("???"))
        {
            foreach (var spec in singleSpecs)
            {
                spec.Value.UpdateValue(SingleSpec.UNDEFINED);
            }
            return;
        }

        // 플레이어 정보 업데이트
        singleSpecs[nameof(PlayerStat.MaxHP)].UpdateValue(currentPlayerStatData.maxHP);
        singleSpecs[nameof(PlayerStat.HealthRezenPer10)].UpdateValue(currentPlayerStatData.healthRezenPer10);
        singleSpecs[nameof(PlayerStat.DefencePoint)].UpdateValue(currentPlayerStatData.defencePoint);
        singleSpecs[nameof(PlayerStat.EvadeProbability)].UpdateValue(currentPlayerStatData.evadeProbability);
        singleSpecs[nameof(PlayerStat.AttackPoint)].UpdateValue(currentPlayerStatData.attackPoint);
        singleSpecs[nameof(PlayerStat.MultiProjectile)].UpdateValue(currentPlayerStatData.multiProjectile);
        singleSpecs[nameof(PlayerStat.AttackSpeed)].UpdateValue(currentPlayerStatData.attackSpeed);
        singleSpecs[nameof(PlayerStat.AttackRange)].UpdateValue(currentPlayerStatData.attackRange);
        singleSpecs[nameof(PlayerStat.CritProbability)].UpdateValue(currentPlayerStatData.critProbability);
        singleSpecs[nameof(PlayerStat.CritPoint)].UpdateValue(currentPlayerStatData.critPoint);
        singleSpecs[nameof(PlayerStat.ExpGainRatio)].UpdateValue(currentPlayerStatData.expGainRatio);
        singleSpecs[nameof(PlayerStat.ExpGainRange)].UpdateValue((int) currentPlayerStatData.expGainRange);
        singleSpecs[nameof(PlayerStat.MoveSpeed)].UpdateValue((int) currentPlayerStatData.moveSpeed);

    }


    /// <summary>
    /// 캐릭터 선택 화면으로 이동되었을 때 실행
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void OnEnter()
    {
        // 카메라를 캐릭터 선택 룸으로 이동
        // TODO :
    }


    /// <summary>
    /// 캐릭터 선택 화면에서 나갈 때 실행
    /// </summary>
    public override void OnExit()
    {
        // 카메라를 원래 위치로 이동
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
        public const int UNDEFINED = -1234567890;

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
            if (value == UNDEFINED)
            {
                valueField.text = "???";
            }
            else
            {
                valueField.text = value.ToString();
            }
            unitField.text = unit;
        }

        public void UpdateValue(int value)
        {
            this.value = value;
            if (value == UNDEFINED)
            {
                valueField.text = "???";
            }
            else
            {
                valueField.text = value.ToString();
            }
        }
    }
}
