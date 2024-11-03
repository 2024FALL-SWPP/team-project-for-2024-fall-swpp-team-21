using System;
using UnityEngine;

[CreateAssetMenu(fileName = "VirusData", menuName = "ScriptableObj/VirusData", order = 1)]
public class VirusData : ScriptableObject
{

    [Header("오브젝트 풀 타입")]
    [SerializeField] public PoolType poolType;

    [Header("최대 체력")]
    [SerializeField] public int maxHP;

    [Header("이동 속도")]
    [SerializeField] public float moveSpeed;

    [Header("드랍 경험치")]
    [SerializeField] public int dropExp;

    [Header("접촉시 데미지")]
    [SerializeField] public int contactDamage;

}
