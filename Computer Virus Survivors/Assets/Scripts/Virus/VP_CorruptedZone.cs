using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VP_CorruptedZone : VirusProjectileBehaviour
{
    private float speed;
    private float maxScale;
    private float existDuration;
    private float debuffDegree;
    private float dotDamagePeriod;

    private Coroutine dotDamageCoroutine = null;

    public void Initialize(int damage, float speed, float maxScale, float existDuration, float debuffDegree, float dotDamagePeriod)
    {
        base.Initialize(damage);

        this.speed = speed;
        this.maxScale = maxScale;
        this.existDuration = existDuration;
        this.debuffDegree = debuffDegree;
        this.dotDamagePeriod = dotDamagePeriod;
    }

    // Start is called before the first frame update
    private void Start()
    {
        transform.localScale = new Vector3(0.0f, 0.01f, 0.0f);
        StartCoroutine(GetBiggerAndSmaller());
    }

    private IEnumerator GetBiggerAndSmaller()
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

        while (true)
        {
            transform.localScale -= speed * Time.deltaTime * new Vector3(1.0f, 0.0f, 1.0f);
            if (transform.localScale.x <= 0.0f)
            {
                break;
            }
            yield return null;
        }

        GameManager.instance.Player.GetComponent<PlayerController>().RestoreMoveSpeed();

        if (dotDamageCoroutine != null)
        {
            StopCoroutine(dotDamageCoroutine);
            dotDamageCoroutine = null;
        }
        PoolManager.instance.ReturnObject(PoolType.VProj_CorruptedZone, gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered corrupted zone");
            other.GetComponent<PlayerController>().DebuffMoveSpeed(debuffDegree);
            dotDamageCoroutine = StartCoroutine(GiveDotDamage(other.GetComponent<PlayerController>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited corrupted zone");
            other.GetComponent<PlayerController>().RestoreMoveSpeed();
            StopCoroutine(dotDamageCoroutine);
        }
    }

    private IEnumerator GiveDotDamage(PlayerController playerController)
    {
        while (true)
        {
            Debug.Log("Player got damage from corrupted zone");
            playerController.GetDamage(damage);
            yield return new WaitForSeconds(dotDamagePeriod);
        }
    }
}
