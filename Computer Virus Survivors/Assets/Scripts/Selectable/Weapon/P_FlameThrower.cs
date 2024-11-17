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

    private Coroutine damageCoroutine;
    private float radius;

    public void Initialize(int damage, float radius)
    {
        this.damage = damage;
        this.radius = radius;
    }

    public void FireOn(float duration)
    {
        damageCoroutine = StartCoroutine(GiveDamage());
        StartCoroutine(Fire(duration));
    }

    private IEnumerator GiveDamage()
    {
        while (true)
        {
            // 부채꼴 범위의 바이러스에게 데미지를 줌
            Vector3 transformOnPlane = new Vector3(transform.position.x, 0, transform.position.z);
            Collider[] colliders = Physics.OverlapSphere(transformOnPlane, radius, virusLayer);
            foreach (Collider collider in colliders)
            {
                Vector3 direction = (collider.transform.position - transformOnPlane).normalized;

                if (Vector3.Angle(transform.forward, direction) < (fireAngle * 0.5f))
                {
                    collider.GetComponent<VirusBehaviour>().GetDamage(damage);
                }
            }
            yield return new WaitForSeconds(tick);
        }
    }

    private IEnumerator Fire(float duration)
    {
        if (flameParticle == null)
        {
            flameParticle = GetComponent<ParticleSystem>();
        }
        var shape = flameParticle.shape;
        shape.angle = fireAngle / 2;
        var main = flameParticle.main;
        main.duration = duration;
        flameParticle.Play();
        yield return new WaitForSeconds(duration);
        StopCoroutine(damageCoroutine);
    }

    protected override void OnTriggerEnter(Collider other)
    {

    }
}
