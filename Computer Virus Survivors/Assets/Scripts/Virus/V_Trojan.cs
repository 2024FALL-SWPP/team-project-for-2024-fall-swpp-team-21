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

    private bool isAttacking = false;
    private bool doNotTrack = false; // 공격 쿨타임 중에 있음

    protected override void OnEnable()
    {
        base.OnEnable();
        SpawnManager.instance.SpawnTurret();
    }

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
        // Maybe only use x and z
        if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            Debug.Log("Trojan is attacking");

            yield return new WaitForSeconds(dashDelay); // 잠시 멈춤
            yield return StartCoroutine(Dash(dashDuration)); // 대시
            yield return new WaitForSeconds(dashDelay); // 잠시 멈춤
            isAttacking = false;

            doNotTrack = true;
            yield return new WaitForSeconds(attackPeriod);
            doNotTrack = false;
        }
    }
}
