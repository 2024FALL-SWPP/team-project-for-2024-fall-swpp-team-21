using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_Adware : VirusBehaviour
{
    [SerializeField] private float attackPeriod;
    [SerializeField] private float attackRange;
    // [SerializeField] private float attackDelay;
    [SerializeField] private int attackDamage;
    [SerializeField] private float projSpeed;
    [SerializeField] private float projHeight;

    private bool canAttack = false;
    // private bool isAttacking = false;
    private float attackTimer = 0.0f;

    private void FixedUpdate()
    {
        if (canAttack && Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            Attack();
        }
        else
        {
            Move();

            if (!canAttack)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackPeriod)
                {
                    canAttack = true;
                }
            }
        }

        rb.velocity = Vector3.zero;
    }

    private void Attack()
    {
        canAttack = false;

        Vector3 projPos = new Vector3(transform.position.x, projHeight, transform.position.z);
        Vector3 projDir = (player.transform.position - transform.position).normalized;

        GameObject proj = PoolManager.instance.GetObject(PoolType.VProj_Mail, projPos, Quaternion.LookRotation(projDir));
        proj.GetComponent<VP_Mail>().Initialize(attackDamage, projSpeed, projDir, projHeight);

        attackTimer = 0.0f;
    }
}