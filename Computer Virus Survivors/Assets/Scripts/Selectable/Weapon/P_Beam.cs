using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Beam : ProjectileBehaviour
{
    [SerializeField] private float speed = 10f;
    private GameObject lightBeam;
    private ParticleSystem particle;
    private Vector3 targetPos;
    private bool isHit;

    public void Initialize(int damage, Vector3 target)
    {
        if (lightBeam == null)
        {
            lightBeam = GetComponentInChildren<MeshRenderer>().gameObject;
            particle = GetComponentInChildren<ParticleSystem>();
        }
        this.damage = damage;
        this.targetPos = target;
        isHit = false;
        lightBeam.SetActive(true);
    }

    private void Update()
    {
        if (targetPos != null)
        {
            transform.LookAt(targetPos);
        }

        if (!isHit)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }

        if (CheckOutOfScreen())
        {
            PoolManager.instance.ReturnObject(PoolType.Proj_Beam, gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (isHit) return;

        if (other.CompareTag("Virus"))
        {
            other.GetComponent<VirusBehaviour>().GetDamage(damage);
        }

        isHit = true;
        lightBeam.SetActive(false);
        particle.Play();
        StartCoroutine(Destroy(particle.main.duration));
    }

    private IEnumerator Destroy(float duration)
    {
        Debug.Log("Beam Destroyed after " + duration + " seconds");
        yield return new WaitForSeconds(duration);
        PoolManager.instance.ReturnObject(PoolType.Proj_Beam, gameObject);
    }
}
