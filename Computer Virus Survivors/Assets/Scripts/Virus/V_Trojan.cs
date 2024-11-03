using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_Trojan : VirusBehaviour
{
    public float attackPeriod = 4;
    public float attackRange = 100;

    private bool isAttacking = false;

    // Temp
    private void Start()
    {
        player = GameObject.Find("Player");
        maxHP = 200;
        currentHP = maxHP;
        moveSpeed = 4f;
        dropExp = 2500;
        contactDamage = 40;

        //StartCoroutine(AttackCoroutine());
    }

    private void Update()
    {
        if (!isAttacking)
        {
            Move();
        }
    }

    // TODO: Implement dash attack
    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            // Maybe just use x and z
            if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
            {
                Debug.Log("Trojan is attacking");
                isAttacking = true;
                // TODO: Attack animation
                yield return new WaitForSeconds(1);
                isAttacking = false;
                yield return new WaitForSeconds(attackPeriod);
            }
        }
    }
}
