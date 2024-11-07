using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

public abstract class SelectableBehaviour : MonoBehaviour
{
    [Header("이름")]
    [SerializeField]
    private string objectName;

    [Header("최대 레벨")]
    [SerializeField]
    private int maxLevel;

    [Header("레벨 별 설명")]
    [Multiline]
    [SerializeField, ReadOnly(true)]
    protected List<string> explanations = new List<string>();

    private GameObject player;
    private int currentLevel = 0;
    protected GameObject Player
    {
        get
        {
            if (player == null)
            {
                player = GameManager.instance.Player;
            }

            return player;
        }
    }

    /// <summary>
    /// 읽기 전용 필드
    /// </summary>
    public string ObjectName { get { return objectName; } }
    public int CurrentLevel { get { return currentLevel; } }
    public int MaxLevel { get { return maxLevel; } }
    public ReadOnlyCollection<string> Explanations => explanations.AsReadOnly();


    /// <summary>
    /// 추상 메소드
    /// </summary>
    public abstract void Initialize();
    protected abstract void LevelUpEffect(int currentLevel);
    protected abstract void InitExplanation();

    public bool IsMaxLevel()
    {
        return currentLevel == maxLevel;
    }


    /// <summary>
    /// 플레이어가 이 아이템을 획득했을 때 호출되는 메소드
    /// </summary>
    public void Acquire()
    {

        if (currentLevel == 0)
        {
            Initialize();
        }

        if (currentLevel < maxLevel)
        {
            currentLevel++;
            LevelUpEffect(currentLevel);
        }
    }


    /// <summary>
    /// 인스펙터 창에서 최대 레벨을 변경했을 때 설명 리스트를 갱신하는 메소드
    /// </summary>
    private void OnValidate()
    {
        if (MaxLevel > 0 && explanations.Count > MaxLevel)
        {
            explanations.RemoveRange(MaxLevel, explanations.Count - MaxLevel);
        }
        else
        {
            for (int i = explanations.Count; i < MaxLevel; i++)
            {
                if (i == 0)
                {
                    explanations.Add($"<처음 습득 시 설명>");
                }
                else
                {
                    explanations.Add($"<레벨 {i + 1} 업그레이드 설명>");
                }

            }
        }

        InitExplanation();
    }

    /// <summary>
    /// Equals 메소드 오버라이드
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>아이템 이름이 같으면 같은 오브젝트</returns>
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SelectableBehaviour info = (SelectableBehaviour) obj;
        return ObjectName == info.ObjectName;
    }

    public override int GetHashCode()
    {
        return ObjectName.GetHashCode();
    }

}
