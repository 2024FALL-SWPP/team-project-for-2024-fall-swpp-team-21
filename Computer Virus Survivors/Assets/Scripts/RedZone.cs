using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone : MonoBehaviour
{
    [SerializeField] private Renderer bigRenderer;
    [SerializeField] private Renderer smallRenderer;

    [SerializeField] private float fadeInSpeed = 1.0f;
    [SerializeField] private float turretSpawnDelay = 1.0f;
    [SerializeField] private float turretSpawnY = -5.0f;

    private void OnEnable()
    {
        StartCoroutine(RedZoneFadeIn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator RedZoneFadeIn()
    {
        Material bigMaterial = bigRenderer.material;
        Material smallMaterial = smallRenderer.material;

        Color color = bigMaterial.color;
        float alpha = 0;
        while (alpha < 1)
        {
            color.a = alpha;
            bigMaterial.color = color;
            smallMaterial.color = color;
            alpha += Time.deltaTime * fadeInSpeed;
            yield return null;
        }

        color.a = 1;
        bigMaterial.color = color;
        smallMaterial.color = color;

        yield return new WaitForSeconds(turretSpawnDelay);

        SpawnTurret();
    }

    private IEnumerator RedZoneFadeOut()
    {
        Material bigMaterial = bigRenderer.material;
        Material smallMaterial = smallRenderer.material;

        Color color = bigMaterial.color;
        float alpha = 1;
        while (alpha > 0)
        {
            color.a = alpha;
            bigMaterial.color = color;
            smallMaterial.color = color;
            alpha -= Time.deltaTime * fadeInSpeed;
            yield return null;
        }

        color.a = 0;
        bigMaterial.color = color;
        smallMaterial.color = color;

        PoolManager.instance.ReturnObject(PoolType.RedZone, gameObject);
    }

    private void SpawnTurret()
    {
        Debug.Log("Turret Spawned");
        GameObject turret = PoolManager.instance.GetObject
        (
            PoolType.Turret,
            transform.position + new Vector3(0, turretSpawnY, 0),
            transform.rotation
        );

        turret.GetComponent<VirusBehaviour>().OnDie += OnTurretDestroyed;
    }

    private void OnTurretDestroyed(VirusBehaviour turret)
    {
        // turret.OnDie -= OnTurretDestroyed;
        StartCoroutine(RedZoneFadeOut());
    }
}
