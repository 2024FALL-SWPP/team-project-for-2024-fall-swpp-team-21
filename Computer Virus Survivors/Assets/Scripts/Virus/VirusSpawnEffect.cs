using UnityEngine;

public class VirusSpawnEffect : MonoBehaviour
{

    private new ParticleSystem particleSystem;
    private float effectSize;
    [SerializeField] private float defaultEffectSize = 0.7f;
    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        effectSize = defaultEffectSize;
    }

    public void SetEffectSize(float size)
    {
        effectSize = size;
    }

    public void SetVirusSize(float virusSize)
    {
        particleSystem.transform.localScale = Vector3.one * virusSize * effectSize;
        transform.position = new Vector3(transform.position.x, virusSize / 2f, transform.position.z);
    }

    public WaitForSeconds PlayAndWaitUntilEffectPeak()
    {
        float spawnDuration = particleSystem.main.duration;

        return new WaitForSeconds(spawnDuration / 2f);
    }

    private void OnParticleSystemStopped()
    {
        effectSize = defaultEffectSize;
        PoolManager.instance.ReturnObject(PoolType.VFX_Virus_SpawnEffect, gameObject);
    }

}
