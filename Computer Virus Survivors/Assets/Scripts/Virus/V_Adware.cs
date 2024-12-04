using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_Adware : VirusBehaviour
{
    [SerializeField] private float attackPeriod;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;
    [SerializeField] private int attackDamage;
    [SerializeField] private float projSpeed;
    [SerializeField] private float projHeight;

    private bool isAttacking = false;
    private bool doNotTrack = false; // 공격 쿨타임 중에 있음

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            Move();
        }

        if (!isAttacking && !doNotTrack && Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            isAttacking = true;
            StartCoroutine(AttackCoroutine());
        }

        rb.velocity = Vector3.zero;
    }

    private IEnumerator AttackCoroutine()
    {
        // Maybe only use x and z
        if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            Debug.Log("Adware attack START");

            yield return new WaitForSeconds(attackDelay); // 잠시 멈춤

            Vector3 projPos = new Vector3(transform.position.x, projHeight, transform.position.z);
            Vector3 projDir = (player.transform.position - transform.position).normalized;

            GameObject proj = PoolManager.instance.GetObject(PoolType.VProj_Mail, projPos, Quaternion.identity);
            proj.GetComponent<VP_Mail>().Initialize(attackDamage, projSpeed, projDir);

            yield return new WaitForSeconds(attackDelay); // 잠시 멈춤
            isAttacking = false;

            doNotTrack = true;
            yield return new WaitForSeconds(attackPeriod);
            doNotTrack = false;
        }
    }
}