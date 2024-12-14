using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class V_Ransomware : VirusBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float attackPeriod = 10.0f;
    [SerializeField] private float attackDelay = 0.5f;

    /* 공격 패턴들을 위한 변수 */
    [Header("Encryption Spike: 나선형 탄막 패턴")]
    [SerializeField] private int eSNum = 6;
    [SerializeField] private float eSHeight = 0.7f;
    [SerializeField] private float eSOffset = 1.0f;
    [SerializeField] private int eSDamage = 10;
    [SerializeField] private float eSSpeed = 10.0f;
    [SerializeField] private float eSRotateSpeed = 180.0f;

    [Header("UI Jam: 방향키 반전")]
    [SerializeField] private float uJDuration = 5.0f;
    [SerializeField] private float uJCooltime = 31.0f;

    [Header("Corrupted Zone: 데미지/디버프 장판 생성")]
    [SerializeField] private Vector2 cZRange = new Vector2(15.0f, 15.0f);
    [SerializeField] private int cZNum = 3;
    [SerializeField] private int cZDamage = 2;
    [SerializeField] private float cZSpeed = 10.0f;
    [SerializeField] private float cZMaxScale = 6.0f;
    [SerializeField] private float cZExistDuration = 5.0f;
    [SerializeField] private float cZDebuffDegree = 0.5f;
    [SerializeField] private float cZDotDamagePeriod = 0.1f;

    [Header("Tracking Bolt: 플레이어를 빠르게 추적하는 번개 느낌의 무언가")]
    [SerializeField] private float tBSpawnDistance = 1.0f;
    [SerializeField] private int tBDamage = 10;
    [SerializeField] private float tBSpeed = 10.0f;
    [SerializeField] private float tBExistDuration = 5.0f;

    [Header("Data Burst: 플레이어를 향해 연속해서 빠르게 탄막 발사")]
    [SerializeField] private float dBPeriod = 0.5f;
    [SerializeField] private int dBNum = 10;
    [SerializeField] private float dBHeight = 0.7f;
    [SerializeField] private float dBOffset = 1.0f;
    [SerializeField] private int dBDamage = 10;
    [SerializeField] private float dBInitialSpeed = 5f;
    [SerializeField] private float dBAcceleration = 0.2f;

    // 이 외에 생각한 패턴: 광역 원 범위 패턴

    private readonly List<Action> attackActions = new List<Action>();
    private bool startAttack = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(AttackCoroutine());
        attackActions.Add(EncryptionSpike);
        attackActions.Add(UIJam);
        attackActions.Add(CorruptedZone);
        attackActions.Add(TrackingBolt);
        attackActions.Add(DataBurst);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        startAttack = false;
        SpawnManager.instance.SpawnTurret();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        if (!startAttack)
        {
            Move();
        }
        rb.velocity = Vector3.zero;
    }

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackPeriod);
            startAttack = true;
            animator.SetTrigger("t_Attack");
            yield return new WaitForSeconds(attackDelay);
            startAttack = false;
            attackActions[UnityEngine.Random.Range(0, attackActions.Count)]();
        }
    }

    private void EncryptionSpike()
    {
        Debug.Log("Encryption Spike!");
        List<GameObject> pfs = new List<GameObject>();
        float angle = 2 * Mathf.PI / eSNum;
        for (int i = 0; i < eSNum; i++)
        {
            float x = eSOffset * Mathf.Cos(angle * i);
            float z = eSOffset * Mathf.Sin(angle * i);
            Vector3 spikePosition = transform.position + new Vector3(x, eSHeight, z);
            Vector3 direction = new Vector3(x, 0, z).normalized;
            GameObject pf = PoolManager.instance.GetObject(PoolType.VProj_EncryptionSpike, spikePosition, Quaternion.LookRotation(direction, Vector3.up));
            pf.GetComponent<VP_EncryptionSpike>().Initialize(eSDamage, eSSpeed, eSRotateSpeed, eSOffset);
        }
    }

    private void UIJam()
    {
        Debug.Log("UI Jam!");
        playerController.ReverseSpeed(uJDuration);
        StartCoroutine(UIJamCoolDown());
    }

    private IEnumerator UIJamCoolDown()
    {
        attackActions.Remove(UIJam);
        yield return new WaitForSeconds(uJCooltime);
        attackActions.Add(UIJam);
    }


    private void CorruptedZone()
    {
        Debug.Log("Corrupted Zone!");
        for (int i = 0; i < cZNum; i++)
        {
            float x = UnityEngine.Random.Range(-cZRange.x, cZRange.x);
            float z = UnityEngine.Random.Range(-cZRange.y, cZRange.y);
            Vector3 position = player.transform.position + new Vector3(x, 0.0f, z);
            GameObject cZ = PoolManager.instance.GetObject(PoolType.VProj_CorruptedZone, position, Quaternion.identity);
            cZ.GetComponent<VP_CorruptedZone>().Initialize(cZDamage, cZSpeed, cZMaxScale, cZExistDuration, cZDebuffDegree, cZDotDamagePeriod);
        }
    }

    private void TrackingBolt()
    {
        Debug.Log("Tracking Bolt!");
        Vector3 spawnPosition = player.transform.position + tBSpawnDistance * UnityEngine.Random.insideUnitSphere;
        GameObject tB = PoolManager.instance.GetObject(PoolType.VProj_TrackingBolt, spawnPosition, Quaternion.identity);
        tB.GetComponent<VP_TrackingBolt>().Initialize(tBDamage, tBSpeed, tBExistDuration);
    }

    private void DataBurst()
    {
        Debug.Log("Data Burst!");
        StartCoroutine(DataBurstCoroutine());
    }

    private IEnumerator DataBurstCoroutine()
    {
        for (int i = 0; i < dBNum; i++)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Vector3 position = transform.position + dBHeight * Vector3.up + dBOffset * direction;
            GameObject dB = PoolManager.instance.GetObject(PoolType.VProj_DataBurst, position, Quaternion.LookRotation(direction, Vector3.up));
            dB.GetComponent<VP_DataBurst>().Initialize(dBDamage, dBInitialSpeed, dBAcceleration);
            yield return new WaitForSeconds(dBPeriod);
        }
    }

    protected override void Die()
    {
        // TODO: Die animation
        base.Die();
        GameManager.instance.GameClear();
    }
}
