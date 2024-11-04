using UnityEngine;
using System.Collections.Generic;
using System;
[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObj/ItemData", order = 0)]
public class ItemData : SelectableData
{
    [Multiline]
    [SerializeField]
    private List<string> explanations = new List<string>();

    [NonSerialized] public string itemName;
    [NonSerialized] public int levelMax;
    [NonSerialized] public List<string> itemExplanations;

    public void Initialize()
    {
        itemName = objectName;
        levelMax = maxLevel;
        currentLevel = 0;
        itemExplanations = explanations;
        Debug.Log($"Item <{itemName}> initialized");
    }

    private void OnValidate()
    {
        if (maxLevel > 0 && explanations.Count + 1 > maxLevel)
        {
            explanations.RemoveRange(maxLevel + 1, explanations.Count - maxLevel - 1);
        }
        else
        {
            for (int i = explanations.Count; i <= maxLevel; i++)
            {
                if (i == 0)
                    explanations.Add($"Placeholder : not used");
                else if (i == 1)
                    explanations.Add($"<처음 습득 시 설명>");
                else
                    explanations.Add($"<레벨 {i} 업그레이드 설명>");
            }
        }
    }
}
