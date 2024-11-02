using System;
using UnityEngine;

public class SelectableData : ScriptableObject
{
    [SerializeField] protected string objectName;
    [SerializeField] protected int maxLevel;
    [NonSerialized] public int currentLevel;
}
