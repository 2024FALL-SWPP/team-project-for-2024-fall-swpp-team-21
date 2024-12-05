using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class V_Trojan : VirusBehaviour
{
    [SerializeField] private float attackPeriod = 4.0f;
    [SerializeField] private float attackRange = 100.0f;
    [SerializeField] private float dashSpeed = 15.0f;
    [SerializeField] private float dashDelay = 0.3f;
    [SerializeField] private float dashDuration = 0.5f;

    private bool canAttack = false;
    private bool isAttacking = false;
    private float attackTimer = 0.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        SpawnManager.instance.SpawnTurret();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            if (canAttack && Vector3.Distance(player.transform.position, transform.position) < attackRange)
            {
                isAttacking = true;
                StartCoroutine(AttackCoroutine());
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
        }

        rb.velocity = Vector3.zero;
    }

    // 돌진 - 방향 전환 없이 이동
    private IEnumerator Dash(float duration)
    {
        Debug.Log("Trojan dash START");

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            //transform.Translate(dashSpeed * Time.deltaTime * Vector3.forward, Space.Self);
            rb.MovePosition(rb.position + dashSpeed * Time.deltaTime * transform.forward);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        Debug.Log("Trojan dash END");
    }

    // TODO: 애니메이션 이용해서 구현
    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        canAttack = false;

        yield return new WaitForSeconds(dashDelay); // 잠시 멈춤
        yield return StartCoroutine(Dash(dashDuration)); // 대시
        yield return new WaitForSeconds(dashDelay); // 잠시 멈춤

        attackTimer = 0.0f;
        isAttacking = false;
    }
}
