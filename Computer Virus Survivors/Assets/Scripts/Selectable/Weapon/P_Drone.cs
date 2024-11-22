using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;


public class P_Drone : ProjectileBehaviour
{
    [SerializeField] private PoolType projectileType;

    // private float fireRadius;
    // private float attackPeriod;

    private Transform hangingTransform;
    private GameObject lookAtTarget;

    // public void Initialize(int damage, float radius, Transform transform, float attackPeriod)
    // {
    //     this.damage = damage;
    //     this.fireRadius = radius;
    //     this.hangingTransform = transform;
    //     this.attackPeriod = attackPeriod;
    //     StartCoroutine(AttackEnemy());
    // }

    public void Initialize(FinalWeaponData finalWeaponData, Transform transform)
    {
        base.Initialize(finalWeaponData);

        this.hangingTransform = transform;
        StartCoroutine(AttackEnemy());
    }

    // public void UpgradeDrone(int damage, float radius, float attackPeriod)
    // {
    //     this.damage = damage;
    //     this.fireRadius = radius;
    //     this.attackPeriod = attackPeriod;
    // }

    private void Update()
    {
        if (hangingTransform != null)
        {
            if (Vector3.Distance(transform.position, hangingTransform.position) > 1f)
            {
                transform.position = Vector3.Lerp(transform.position, hangingTransform.position, Time.deltaTime * 2f);
            }
        }

        if (lookAtTarget != null)
        {
            Vector3 targetDir = Vector3.ProjectOnPlane(lookAtTarget.transform.position - transform.position, Vector3.up).normalized + new Vector3(0, -0.2f, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * 10f);
        }
    }

    private IEnumerator AttackEnemy()
    {
        GameObject target = null;
        while (true)
        {
            bool isFound = false;

            void OnEnemyFound(GameObject t)
            {
                target = t;
                isFound = true;
                lookAtTarget = t;
            }

            StartCoroutine(MonsterScanner.SearchEnemyCoroutine(this.transform, finalWeaponData.attackRange, OnEnemyFound));

            yield return new WaitUntil(() => isFound);

            if (target != null)
            {
                P_Beam beam = PoolManager.instance.GetObject(projectileType, transform.position, transform.rotation).GetComponent<P_Beam>();
                beam.Initialize(finalWeaponData, target.transform.position);
            }

            yield return new WaitForSeconds(finalWeaponData.attackPeriod);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {

    }
}
