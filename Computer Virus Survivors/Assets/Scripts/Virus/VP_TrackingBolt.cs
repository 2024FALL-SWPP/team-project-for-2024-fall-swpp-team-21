using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VP_TrackingBolt : VirusProjectileBehaviour
{
    private GameObject player;
    private float speed;
    private float existDuration;
    private bool canDamage = false;

    public void Initialize(int damage, float speed, float existDuration)
    {
        base.Initialize(damage);

        player = GameManager.instance.Player;
        this.speed = speed;
        this.existDuration = existDuration;

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.5f);
        canDamage = true;

        float elapsedTime = 0;
        while (elapsedTime < existDuration)
        {
            Vector3 moveDirection = Vector3.ProjectOnPlane(
                (player.transform.position - transform.position).normalized,
                Vector3.up);
            transform.Translate(speed * Time.deltaTime * moveDirection, Space.World);
            transform.rotation = Quaternion.LookRotation(moveDirection);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            other.GetComponent<PlayerController>().GetDamage(damage);
            Destroy(gameObject);
        }
    }
}
