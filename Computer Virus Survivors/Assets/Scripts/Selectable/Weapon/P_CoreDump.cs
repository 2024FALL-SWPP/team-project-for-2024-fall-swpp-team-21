using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_CoreDump : PlayerProjectileBehaviour
{
    [SerializeField] private float dumpSpeed = 3.0f;
    [SerializeField] private LayerMask virusLayer;
    private Vector3 fallingDirection;
    private float dumpRadius;

    // public void Initialize(int damage, Vector3 fallingDirection)
    // {
    //     base.Initialize(damage);

    //     if (animator == null)
    //     {
    //         animator = GetComponent<Animator>();
    //     }
    //     animator.SetBool("isGrounded_b", false);
    //     this.fallingDirection = fallingDirection;
    //     StartCoroutine(FallAndExplode());
    // }

    public void Initialize(FinalWeaponData finalWeaponData, Vector3 fallingDirection)
    {
        base.Initialize(finalWeaponData);
        dumpRadius = finalWeaponData.attackRange;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.SetBool("isGrounded_b", false);
        this.fallingDirection = fallingDirection;
        StartCoroutine(FallAndExplode());
    }

    private IEnumerator FallAndExplode()
    {
        while (transform.position.y > 0)
        {
            transform.Translate(dumpSpeed * Time.deltaTime * fallingDirection, Space.World);
            yield return null;
        }

        animator.SetBool("isGrounded_b", true);
        Collider[] colliders = Physics.OverlapSphere(transform.position, dumpRadius, virusLayer);
        bool criticalExist = false;
        foreach (Collider collider in colliders)
        {
            // collider.GetComponent<VirusBehaviour>().GetDamage(finalWeaponData.GetFinalDamage(), finalWeaponData.knockbackTime);
            collider.GetComponent<VirusBehaviour>().GetDamage(finalWeaponData.GetDamageData(out bool isCritical));
            if (isCritical)
            {
                criticalExist = true;
            }
        }
        PlayAttackEffect(transform.position, Quaternion.identity, criticalExist, dumpRadius);
        yield return new WaitForSeconds(1.0f);
        PoolManager.instance.ReturnObject(PoolType.Proj_CoreDump, gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {

    }
}
