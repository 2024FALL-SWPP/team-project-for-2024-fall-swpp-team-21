using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObj/WeaponData", order = 0)]
public class WeaponData : SelectableData
{

    /// <summary>
    /// 초기값 설정
    /// </summary>

    [Header("발사체 프리팹")]
    [SerializeField]
    private GameObject projectilePrefab;

    [Header("기본 공격력(int)")]
    [SerializeField]
    private int damage;   // 기본 공격력

    [Header("기본 발사체 개수(int, 개)")]
    [SerializeField]
    private int multiProjectile;  // 기본 다중 발사체 수

    [Header("기본 공격 주기(float, 초)")]
    [Min(0)]
    [SerializeField]
    private float attackPeriod;   // 기본 공격 주기

    [Header("기본 공격 범위(float)")]
    [Min(0)]
    [SerializeField]
    private float attackRange;    // 기본 공격 범위

    [Header("기본 추가 치명타 확률(int, %)")]
    [SerializeField]
    private int addiCritProbability;  // 기본 치명타 공격 확률

    [Header("기본 추가 치명타 대미지(int, %)")]
    [SerializeField]
    private int addiCritPoint;    // 기본 치명타 공격 대미지 배율

    [Multiline]
    [SerializeField]
    private List<string> explanations = new List<string>();

    /// <summary>
    /// 런타임 변수
    /// </summary>

    [NonSerialized] public string weaponName;
    [NonSerialized] public ProjectileBehaviour projectile;
    [NonSerialized] public int basicDamage;
    [NonSerialized] public int basicMultiProjectile;
    [NonSerialized] public float basicAttackPeriod;
    [NonSerialized] public float basicAttackRange;
    [NonSerialized] public int basicAddiCritProbability;
    [NonSerialized] public int basicAddiCritPoint;
    [NonSerialized] public int levelMax;

    public void Initialize()
    {
        weaponName = objectName;
        projectile = projectilePrefab.GetComponent<ProjectileBehaviour>();
        basicDamage = damage;
        basicMultiProjectile = multiProjectile;
        basicAttackPeriod = attackPeriod;
        basicAttackRange = attackRange;
        basicAddiCritProbability = addiCritProbability;
        basicAddiCritPoint = addiCritPoint;
        currentLevel = 0;
        levelMax = maxLevel;
        Debug.Log($"Weapon <{weaponName}> initialized");
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
