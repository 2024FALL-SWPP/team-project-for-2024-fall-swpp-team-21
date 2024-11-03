using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_Trojan : VirusBehaviour
{
    public float attackPeriod = 4.0f;
    public float attackRange = 100.0f;
    public float defaultSpeed = 4.0f;
    public float dashSpeed = 15.0f;

    private bool isAttacking = false;
    private bool doNotTrack = false; // 공격 쿨타임 중에 있음

    // Temp
    private void Start()
    {
        maxHP = 200;
        currentHP = maxHP;
        moveSpeed = defaultSpeed;
        dropExp = 2500;
        contactDamage = 40;
    }

    private void Update()
    {
        if (!isAttacking)
        {
            Move();
        }
        else
        {
            Dash();
        }

        if (!isAttacking && !doNotTrack && Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            isAttacking = true;
            StartCoroutine(AttackCoroutine());
        }
    }

    // 돌진 - 방향 전환 없이 이동
    private void Dash()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward, Space.Self);
    }

    // TODO: 애니메이션 이용해서 구현
    private IEnumerator AttackCoroutine()
    {
        // Maybe only use x and z
        if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            Debug.Log("Trojan is attacking");

            moveSpeed = 0;
            yield return new WaitForSeconds(0.3f); // 잠시 멈춤
            moveSpeed = dashSpeed;
            yield return new WaitForSeconds(0.5f); // 돌진
            moveSpeed = 0;
            yield return new WaitForSeconds(0.3f); // 잠시 멈춤
            isAttacking = false;
            doNotTrack = true;
            moveSpeed = defaultSpeed;
            yield return new WaitForSeconds(attackPeriod);
            doNotTrack = false;
        }
    }
}
