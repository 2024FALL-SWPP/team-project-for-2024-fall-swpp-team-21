using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class V_Ransomware : VirusBehaviour
{
    [SerializeField] private float attackPeriod = 10.0f;
    [SerializeField] private float attackDelay = 0.5f;

    [Header("Encryption Spike: 플레이어에게 큰 탄환 발사")]
    [SerializeField] private GameObject eSPrefab;
    [SerializeField] private int eSDamage = 10;
    [SerializeField] private float eSSpeed = 10.0f;
    [SerializeField] private float eSDebuffDegree = 0.5f;
    [SerializeField] private float eSDebuffDuration = 3.0f;

    [Header("UI Jam: 방향키 반전")]
    [SerializeField] private float uJDuration = 5.0f;
    [SerializeField] private float uJCooltime = 31.0f;

    [Header("Corrupted Zone: 데미지/디버프 장판 생성")]
    [SerializeField] private GameObject cZPrefab;
    [SerializeField] private Vector2 cZRange = new Vector2(15.0f, 15.0f);
    [SerializeField] private int cZNum = 3;
    [SerializeField] private int cZDamage = 1;
    [SerializeField] private float cZSpeed = 10.0f;
    [SerializeField] private float cZMaxScale = 6.0f;
    [SerializeField] private float cZExistDuration = 5.0f;
    [SerializeField] private float cZDebuffDegree = 0.5f;
    [SerializeField] private float cZDebuffDuration = 5.0f;

    private readonly List<Action> attackActions = new List<Action>();
    private bool startAttack = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(AttackCoroutine());
        attackActions.Add(EncryptionSpike);
        attackActions.Add(UIJam);
        attackActions.Add(CorruptedZone);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SpawnManager.instance.SpawnTurret();
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
            yield return new WaitForSeconds(attackDelay);
            startAttack = false;
            attackActions[2]();//UnityEngine.Random.Range(0, attackActions.Count)]();
        }
    }

    private void EncryptionSpike()
    {
        Debug.Log("Encryption Spike!");
        GameObject pf = Instantiate(eSPrefab, transform.position, transform.rotation);
        pf.GetComponent<VP_EncryptionSpike>().Initialize(transform.forward, eSDamage, eSSpeed, eSDebuffDegree, eSDebuffDuration);
    }

    private void UIJam()
    {
        Debug.Log("UI Jam!");
        playerController.BuffMoveSpeed(-1, uJDuration);
        StartCoroutine(CoolDown());
    }

    private void CorruptedZone()
    {
        Debug.Log("Corrupted Zone!");
        for (int i = 0; i < cZNum; i++)
        {
            float x = UnityEngine.Random.Range(-cZRange.x, cZRange.x);
            float z = UnityEngine.Random.Range(-cZRange.y, cZRange.y);
            Vector3 position = player.transform.position + new Vector3(x, 0.0f, z);
            GameObject cZ = Instantiate(cZPrefab, position, cZPrefab.transform.rotation);
            cZ.GetComponent<VP_CorruptedZone>().Initialize(cZDamage, cZSpeed, cZMaxScale, cZExistDuration, cZDebuffDegree, cZDebuffDuration);
        }
    }

    private IEnumerator CoolDown()
    {
        attackActions.Remove(UIJam);
        Debug.Log(attackActions.Count);
        yield return new WaitForSeconds(uJCooltime);
        attackActions.Add(UIJam);
    }


    protected override void Die()
    {
        // TODO: Die animation
        PoolManager.instance.ReturnObject(virusData.poolType, gameObject);
        GameManager.instance.GameClear();
    }
}
