using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_Ransomware : VirusBehaviour
{
    [SerializeField] private float attackPeriod = 10.0f;

    private List<Action> attackActions = new List<Action>();
    // or List<IEnumerator>

    private void OnEnable()
    {
        StartCoroutine(AttackCoroutine());
        attackActions.Add(EncryptionSpike);
        attackActions.Add(UIJam);
        attackActions.Add(CorruptedZone);
        attackActions.Add(FirewallBarricade);
    }

    private void Update()
    {
        Move();
    }

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackPeriod);
            //attackActions[UnityEngine.Random.Range(0, attackActions.Count)]();
            attackActions[1]();
        }
    }

    private void EncryptionSpike()
    {
        Debug.Log("Encryption Spike!");
    }

    private void UIJam()
    {
        Debug.Log("UI Jam!");
        playerController.BuffStat(nameof(PlayerStat.MoveSpeed), -1, 5.0f);
    }

    private void CorruptedZone()
    {
        Debug.Log("Corrupted Zone!");
    }

    private void FirewallBarricade()
    {
        Debug.Log("Firewall Barricade!");
    }
}
