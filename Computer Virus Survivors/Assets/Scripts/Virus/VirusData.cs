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

    [Header("드랍 테이블")]
    [SerializeField] public DropTable dropTable;

    [Header("드랍 경험치")]
    [SerializeField] public int dropExp;

    [Header("접촉시 데미지")]
    [SerializeField] public int contactDamage;

    [Header("넉백 속도")]
    [SerializeField] public float knockbackSpeed;

    [Header("넉백 색상")]
    [SerializeField] public Material knockbackColor;
}
