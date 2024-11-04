using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VP_CorruptedZone : MonoBehaviour
{
    private int damage;
    private float speed;
    private float maxScale;
    private float existDuration;
    private float debuffDegree;
    private float debuffDuration;

    public void Initialize(int damage, float speed, float maxScale, float existDuration, float debuffDegree, float debuffDuration)
    {
        this.damage = damage;
        this.speed = speed;
        this.maxScale = maxScale;
        this.existDuration = existDuration;
        this.debuffDegree = debuffDegree;
        this.debuffDuration = debuffDuration;
    }

    // Start is called before the first frame update
    private void Start()
    {
        transform.localScale = new Vector3(0.0f, 0.01f, 0.0f);
        StartCoroutine(GetBigger());
    }

    private IEnumerator GetBigger()
    {
        while (true)
        {
            transform.localScale += speed * Time.deltaTime * new Vector3(1.0f, 0.0f, 1.0f);
            if (transform.localScale.x >= maxScale)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(existDuration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.GetComponent<PlayerController>().GetDamage(damage);
            other.GetComponent<PlayerController>().BuffMoveSpeed(debuffDegree, debuffDuration);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GetDamage(damage);
        }
    }
}
