using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Beam : ProjectileBehaviour
{
    [SerializeField] private float speed = 10f;
    private GameObject lightBeam;
    private ParticleSystem particle;
    private Transform target;
    private bool isHit;

    public void Initialize(int damage, Transform target)
    {
        if (lightBeam == null)
        {
            lightBeam = GetComponentInChildren<MeshRenderer>().gameObject;
            particle = GetComponentInChildren<ParticleSystem>();
        }
        this.damage = damage;
        this.target = target;
        isHit = false;
        lightBeam.SetActive(true);
    }

    private void Update()
    {
        if (!isHit)
        {
            transform.LookAt(target);
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
            if (CheckOutOfScreen())
            {
                PoolManager.instance.ReturnObject(PoolType.Proj_Beam, gameObject);
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
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
        yield return new WaitForSeconds(duration);
        PoolManager.instance.ReturnObject(PoolType.Proj_Beam, gameObject);
    }
}
