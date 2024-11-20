using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class P_Drone : ProjectileBehaviour
{
    [SerializeField] private float tick = 1f;

    private Coroutine currentCoroutine;
    private float fireRadius = 5.0f;

    private Transform hangingTransform;
    private Transform lookAtTransform;

    public void Initialize(int damage, float radius, Transform transform)
    {
        this.damage = damage;
        this.fireRadius = radius;
        this.hangingTransform = transform;
        currentCoroutine = StartCoroutine(MonsterScanner.SearchEnemyCoroutine(this.transform, fireRadius, EnemyFound));
    }

    public void UpgradeDrone(int damage, float radius, Transform transform)
    {
        this.damage = damage;
        this.fireRadius = radius;
        this.hangingTransform = transform;
    }

    private void Update()
    {
        if (hangingTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, hangingTransform.position, 0.1f);
        }

        if (lookAtTransform != null)
        {
            Vector3 targetDir = Vector3.ProjectOnPlane(lookAtTransform.position - transform.position, Vector3.up).normalized + new Vector3(0, -0.2f, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDir), 0.1f);
        }
    }

    private void EnemyFound(GameObject target)
    {
        lookAtTransform = target.transform;
        target.GetComponent<VirusBehaviour>().OnDie += OnTargetDied;
        currentCoroutine = StartCoroutine(AttackEnemy(target, EnemyMissed));
    }

    private void EnemyMissed(GameObject virus)
    {
        lookAtTransform = null;
        virus.GetComponent<VirusBehaviour>().OnDie -= OnTargetDied;
        currentCoroutine = StartCoroutine(MonsterScanner.SearchEnemyCoroutine(this.transform, fireRadius, EnemyFound));
    }

    private IEnumerator AttackEnemy(GameObject target, Action<GameObject> callback)
    {
        while (true)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > fireRadius)
            {
                callback(target);
                yield break;
            }

            target.GetComponent<VirusBehaviour>().GetDamage(damage);
            yield return new WaitForSeconds(tick);
        }
    }

    private void OnTargetDied(VirusBehaviour virus)
    {
        StopCoroutine(currentCoroutine);
        EnemyMissed(virus.gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {

    }
}
