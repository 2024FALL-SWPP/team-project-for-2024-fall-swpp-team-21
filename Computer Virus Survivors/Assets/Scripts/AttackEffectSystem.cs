using System.Collections;
using UnityEngine;

public enum AttackEffectType
{
    Basic,
    Basic2,
    Basic3,
    Explosion,
    Ice,
    Lightning,
    Magic,

}

public class AttackEffect
{

    public void Play(Vector3 position, AttackEffectType type = AttackEffectType.Basic)
    {
        PlayEffect(position, Quaternion.identity, type);
    }

    public void Play(Vector3 position, Quaternion rotation, AttackEffectType type)
    {
        PlayEffect(position, rotation, type);
    }

    private void PlayEffect(Vector3 position, Quaternion rotation, AttackEffectType type)
    {
        GameObject effect = PoolManager.instance.GetObject(PoolMapping(type), position, rotation);
        effect.GetComponent<ParticleSystem>().Play();
    }

    public static PoolType PoolMapping(AttackEffectType type)
    {
        switch (type)
        {
            case AttackEffectType.Basic:
                return PoolType.VFX_BasicHit1;
            case AttackEffectType.Basic2:
                return PoolType.VFX_BasicHit2;
            case AttackEffectType.Basic3:
                return PoolType.VFX_BasicHit3;
            case AttackEffectType.Explosion:
                return PoolType.VFX_Explosion;
            case AttackEffectType.Ice:
                return PoolType.VFX_IceHit;
            case AttackEffectType.Lightning:
                return PoolType.VFX_LightningHit;
            case AttackEffectType.Magic:
                return PoolType.VFX_MagicHit;
            default:
                return PoolType.VFX_BasicHit1;
        }
    }
}

public class AttackEffectSystem : MonoBehaviour
{
    private ParticleSystem particle;
    [SerializeField] private AttackEffectType effectType;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleSystemStopped()
    {
        PoolManager.instance.ReturnObject(AttackEffect.PoolMapping(effectType), gameObject);
    }
}
