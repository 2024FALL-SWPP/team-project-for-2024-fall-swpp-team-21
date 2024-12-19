using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VP_DataBurst : VirusProjectileBehaviour
{
    [SerializeField] private SFXPreset hitSFXPreset;
    private float speed;
    private float acceleration;

    public void Initialize(int damage, float initialSpeed, float acceleration)
    {
        base.Initialize(damage);

        speed = initialSpeed;
        this.acceleration = acceleration;

        SphereCollider collider = GetComponent<SphereCollider>();
        float offsetY = -transform.position.y / transform.lossyScale.y;
        collider.center = new Vector3(collider.center.x, offsetY, collider.center.z);
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward, Space.Self);
        speed += acceleration * Time.deltaTime;

        // if (CheckOutOfScreen())
        // {
        //     PoolManager.instance.ReturnObject(PoolType.VProj_DataBurst, gameObject);
        // }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hitSFXPreset.Play();
            other.GetComponent<PlayerController>().GetDamage(damage);
            PoolManager.instance.ReturnObject(PoolType.VProj_DataBurst, gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            PoolManager.instance.ReturnObject(PoolType.VProj_DataBurst, gameObject);
        }
    }
}
