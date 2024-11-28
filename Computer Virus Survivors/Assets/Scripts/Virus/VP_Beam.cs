using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VP_Beam : VirusProjectileBehaviour
{
    private float speed;
    private GameObject lightBeam;
    private ParticleSystem particle;
    private Vector3 targetPos;
    private bool isHit;

    public void Initialize(Vector3 target, float speed, int damage)
    {
        base.Initialize(damage);

        if (lightBeam == null)
        {
            lightBeam = GetComponentInChildren<MeshRenderer>().gameObject;
            particle = GetComponentInChildren<ParticleSystem>();
        }
        this.targetPos = target;
        this.speed = speed;

        isHit = false;
        lightBeam.SetActive(true);
        transform.LookAt(targetPos);
    }

    private void Update()
    {
        if (!isHit)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);

            if (transform.position.y < 0)
            {
                StartCoroutine(Destroy(particle.main.duration));
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (isHit || other.CompareTag("Magnet") || other.CompareTag("Virus"))
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GetDamage(damage);
        }

        StartCoroutine(Destroy(particle.main.duration));
    }

    private IEnumerator Destroy(float duration)
    {
        Debug.Log("Beam Destroyed after " + duration + " seconds");

        isHit = true;
        lightBeam.SetActive(false);
        particle.Play();

        yield return new WaitForSeconds(duration);
        PoolManager.instance.ReturnObject(PoolType.VProj_Beam, gameObject);
    }
}
