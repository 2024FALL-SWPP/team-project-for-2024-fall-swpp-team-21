using System;
using UnityEngine;

[CreateAssetMenu(fileName = "VirusData", menuName = "ScriptableObj/VirusData", order = 1)]
public class VirusData : ScriptableObject
{

    [Header("경험치 프리팹")]
    [SerializeField] public GameObject expPrefab;

    [Header("최대 체력")]
    [SerializeField] public int maxHP;

    [Header("이동 속도")]
    [SerializeField] public float moveSpeed;

    [Header("드랍 경험치")]
    [SerializeField] public int dropExp;

    [Header("접촉시 데미지")]
    [SerializeField] public int contactDamage;

}
