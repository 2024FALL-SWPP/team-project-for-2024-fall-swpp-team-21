using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class P_PacketStream : PlayerProjectileBehaviour
{
    [SerializeField] private float bulletSpeed;

    private void OnEnable()
    {
        CapsuleCollider collider = GetComponent<CapsuleCollider>();

        float currentWorldY = transform.position.y;
        float offsetY = -currentWorldY / transform.lossyScale.y;

        collider.center = new Vector3(collider.center.x, offsetY, collider.center.z);
    }

    private void Update()
    {
        transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
        if (CheckOutOfScreen())
        {
            PoolManager.instance.ReturnObject(PoolType.Proj_PacketStream, gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Virus"))
        {
            other.GetComponent<VirusBehaviour>().GetDamage(finalWeaponData.GetDamageData(out bool isCritical));
            PlayAttackEffect(other.ClosestPoint(transform.position) + new Vector3(0, transform.position.y, 0), Quaternion.identity, isCritical);
            PoolManager.instance.ReturnObject(PoolType.Proj_PacketStream, gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            PoolManager.instance.ReturnObject(PoolType.Proj_PacketStream, gameObject);
        }
    }
}
