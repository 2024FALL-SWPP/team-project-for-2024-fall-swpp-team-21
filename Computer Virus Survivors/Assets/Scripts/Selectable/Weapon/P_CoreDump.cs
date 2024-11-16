using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_CoreDump : ProjectileBehaviour
{
    [SerializeField] private float dumpSpeed = 3.0f;
    [SerializeField] private float dumpRadius = 3.0f;
    [SerializeField] private LayerMask virusLayer;

    public override void Initialize(int damage)
    {
        base.Initialize(damage);

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.SetBool("isGrounded_b", false);
        StartCoroutine(FallAndExplode());
    }

    private IEnumerator FallAndExplode()
    {
        while (transform.position.y > 0)
        {
            transform.Translate(dumpSpeed * Time.deltaTime * Vector3.down, Space.World);
            yield return null;
        }

        animator.SetBool("isGrounded_b", true);
        Collider[] colliders = Physics.OverlapSphere(transform.position, dumpRadius, virusLayer);
        foreach (Collider collider in colliders)
        {
            collider.GetComponent<VirusBehaviour>().GetDamage(damage);
        }
        yield return new WaitForSeconds(1.0f);
        PoolManager.instance.ReturnObject(PoolType.Proj_CoreDump, gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {

    }
}
