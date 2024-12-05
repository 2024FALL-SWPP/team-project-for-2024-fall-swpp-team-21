using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VP_Mail : VirusProjectileBehaviour
{
    [SerializeField] private float spinSpeed;
    private float speed;
    private Vector3 direction;

    public void Initialize(int damage, float speed, Vector3 direction, float height)
    {
        base.Initialize(damage);
        this.speed = speed;
        this.direction = direction;

        SphereCollider collider = GetComponent<SphereCollider>();
        collider.center = new Vector3(collider.center.x, -height, collider.center.z);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        transform.Translate(speed * Time.deltaTime * direction, Space.World);

        if (CheckOutOfScreen())
        {
            PoolManager.instance.ReturnObject(PoolType.VProj_Mail, gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GetDamage(damage);
            PoolManager.instance.ReturnObject(PoolType.VProj_Mail, gameObject);
        }
    }
}
