using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VP_EncryptionSpike : VirusProjectileBehaviour
{
    [SerializeField] private SFXPreset hitSFXPreset;
    private float speed;
    private float rotateSpeed;
    private float startOffset;

    private bool canDamage = false;
    private Coroutine attackCoroutine = null;

    public void Initialize(int damage, float speed, float rotateSpeed, float startOffset)
    {
        base.Initialize(damage);

        this.speed = speed;
        this.rotateSpeed = rotateSpeed;
        this.startOffset = startOffset;

        SphereCollider collider = GetComponent<SphereCollider>();
        float offsetY = -transform.position.y / transform.lossyScale.y;
        collider.center = new Vector3(collider.center.x, offsetY, collider.center.z);

        attackCoroutine = StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        canDamage = true;
        float elapsedTime = 0;
        float aliveTime = 360.0f / rotateSpeed - startOffset / speed;
        while (elapsedTime < aliveTime)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward, Space.Self);
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        PoolManager.instance.ReturnObject(PoolType.VProj_EncryptionSpike, gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            other.GetComponent<PlayerController>().GetDamage(damage);
            hitSFXPreset.Play();
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            PoolManager.instance.ReturnObject(PoolType.VProj_EncryptionSpike, gameObject);
        }
    }
}
