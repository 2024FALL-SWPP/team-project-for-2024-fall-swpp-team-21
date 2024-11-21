using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class V_Ransomware : VirusBehaviour
{
    [SerializeField] private float attackPeriod = 5.0f;
    [SerializeField] private float attackDelay = 0.5f;

    [SerializeField] private GameObject encryptionSpikePf;
    [SerializeField] private int eSDamage = 10;
    [SerializeField] private float eSSpeed = 10.0f;
    [SerializeField] private float eSDebuffDegree = 0.5f;
    [SerializeField] private float eSDebuffDuration = 3.0f;

    [SerializeField] private float uIJamDuration = 10.0f;

    [SerializeField] private GameObject corruptedZonePf;
    [SerializeField] private Vector2 cZRange = new Vector2(15.0f, 15.0f);
    [SerializeField] private int cZDamage = 1;
    [SerializeField] private float cZSpeed = 10.0f;
    [SerializeField] private float cZMaxScale = 6.0f;
    [SerializeField] private float cZExistDuration = 5.0f;
    [SerializeField] private float cZDebuffDegree = 0.5f;
    [SerializeField] private float cZDebuffDuration = 5.0f;

    [SerializeField] private GameObject firewallBarricadePf;
    [SerializeField] private float fBDuration = 7.0f;

    [SerializeField] private GameObject stage1Piece;

    private readonly List<Action> attackActions = new List<Action>();
    // or List<IEnumerator>
    private bool startAttack = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(AttackCoroutine());
        attackActions.Add(EncryptionSpike);
        attackActions.Add(UIJam);
        attackActions.Add(CorruptedZone);
        attackActions.Add(FirewallBarricade);
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
            attackActions[UnityEngine.Random.Range(0, attackActions.Count)]();
        }
    }

    private void EncryptionSpike()
    {
        Debug.Log("Encryption Spike!");
        GameObject pf = Instantiate(encryptionSpikePf, transform.position, transform.rotation);
        pf.GetComponent<VP_EncryptionSpike>().Initialize(transform.forward, eSDamage, eSSpeed, eSDebuffDegree, eSDebuffDuration);
    }

    private void UIJam()
    {
        Debug.Log("UI Jam!");
        playerController.BuffMoveSpeed(-1, uIJamDuration);
    }

    private void CorruptedZone()
    {
        Debug.Log("Corrupted Zone!");
        for (int i = 0; i < 3; i++)
        {
            float x = UnityEngine.Random.Range(-cZRange.x, cZRange.x);
            float z = UnityEngine.Random.Range(-cZRange.y, cZRange.y);
            Vector3 position = player.transform.position + new Vector3(x, 0.0f, z);
            GameObject cZ = Instantiate(corruptedZonePf, position, corruptedZonePf.transform.rotation);
            cZ.GetComponent<VP_CorruptedZone>().Initialize(cZDamage, cZSpeed, cZMaxScale, cZExistDuration, cZDebuffDegree, cZDebuffDuration);
        }
    }

    private void FirewallBarricade()
    {
        Debug.Log("Firewall Barricade!");
        StartCoroutine(FirewallBarricadeCoroutine());
    }

    private IEnumerator FirewallBarricadeCoroutine()
    {
        GameObject barricade = Instantiate(firewallBarricadePf, transform);
        yield return new WaitForSeconds(fBDuration);
        Destroy(barricade);
    }

    // 죽을 때 세미콜론 아이템 드롭
    protected override void Die()
    {
        base.Die();  // exp gem 드롭 안할거면 이부분 지우고 새로 써야함
        Instantiate(stage1Piece, transform.position, stage1Piece.transform.rotation);
        Stage1Goal.instance.OnBossDead();
        // TODO: 퍼즐 올라옴
    }
}
