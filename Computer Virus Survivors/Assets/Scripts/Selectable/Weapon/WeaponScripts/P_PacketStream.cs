using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class P_PacketStream : ProjectileBehaviour
{
    [SerializeField] private float bulletSpeed;

    private void OnEnable()
    {
        SphereCollider collider = GetComponent<SphereCollider>();

        float currentWorldY = transform.position.y;
        float offsetY = -currentWorldY / transform.lossyScale.y;

        collider.center = new Vector3(collider.center.x, offsetY, collider.center.z);
    }

    private void OnBecameInvisible()
    {
        PoolManager.instance.ReturnObject(PoolType.Proj_PacketStream, gameObject);
    }

    private void Update()
    {
        transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Virus"))
        {
            other.GetComponent<VirusBehaviour>().GetDamage(damage);
            PoolManager.instance.ReturnObject(PoolType.Proj_PacketStream, gameObject);
        }
    }
}
