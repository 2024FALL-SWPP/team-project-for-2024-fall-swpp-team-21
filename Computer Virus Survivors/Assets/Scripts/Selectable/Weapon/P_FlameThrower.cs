using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_FlameThrower : ProjectileBehaviour
{
    [SerializeField] private LayerMask virusLayer;
    [SerializeField] private float fireAngle = 120.0f;
    [SerializeField] private float tick = 0.5f;
    private ParticleSystem flameParticle;

    // private float radius;

    // public void Initialize(int damage, float radius)
    // {
    //     this.damage = damage;
    //     this.radius = radius;
    // }

    public void FireOn(float duration)
    {
        StartCoroutine(Fire(duration));
    }


    private IEnumerator Fire(float duration)
    {
        float radius = finalWeaponData.attackRange;

        if (flameParticle == null)
        {
            flameParticle = GetComponent<ParticleSystem>();
        }
        var shape = flameParticle.shape;
        shape.angle = fireAngle / 2;
        var main = flameParticle.main;
        main.duration = duration;
        main.startLifetime = 0.5f * radius / 3;
        var emission = flameParticle.emission;
        emission.rateOverTime = 200 * Mathf.Pow(radius / 3, 2);
        flameParticle.Play();

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {

            // 부채꼴 범위의 바이러스에게 데미지를 줌
            Vector3 transformOnPlane = new Vector3(transform.position.x, 0, transform.position.z);
            Collider[] colliders = Physics.OverlapSphere(transformOnPlane, radius, virusLayer);
            foreach (Collider collider in colliders)
            {
                Vector3 direction = (collider.transform.position - transformOnPlane).normalized;

                if (Vector3.Angle(transform.forward, direction) < (fireAngle * 0.5f))
                {
                    collider.GetComponent<VirusBehaviour>().GetDamage(finalWeaponData.GetFinalDamage());
                }
            }
            yield return new WaitForSeconds(tick);
            elapsedTime += tick;
        }

    }

    protected override void OnTriggerEnter(Collider other)
    {

    }
}
