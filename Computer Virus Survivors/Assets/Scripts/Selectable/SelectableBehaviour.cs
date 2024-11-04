using UnityEngine;
using System.Collections.Generic;

public abstract class SelectableBehaviour : MonoBehaviour
{
    [Header("이름")]
    [SerializeField]
    private string itemName;

    [Header("최대 레벨")]
    [SerializeField]
    private int maxLevel;

    [Header("레벨 별 설명")]
    [Multiline]
    [SerializeField]
    private List<string> explanations = new List<string>();

    private int currentLevel;

    public GameObject Player { get; private set; }
    public string ObjectName { get { return itemName; } }
    public int MaxLevel { get { return maxLevel; } }
    public List<string> Explanations { get { return explanations; } }

    public abstract void Initialize();
    protected abstract void LevelUpEffect(int currentLevel);

    public bool IsMaxLevel()
    {
        return currentLevel == maxLevel;
    }

    public void GetSelectable(PlayerController player)
    {

        if (Player == null)
        {
            Player = player.gameObject;
            currentLevel = 1;
            Initialize();
            return;
        }

        if (currentLevel > 0 && currentLevel < maxLevel)
        {
            currentLevel++;
            LevelUpEffect(currentLevel);
        }
    }


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
    }

}
