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

    public void Play(Vector3 position, AttackEffectType type = AttackEffectType.Basic, bool isCritical = false)
    {
        PlayEffect(position, Quaternion.identity, type, isCritical);
    }

    public void Play(Vector3 position, Quaternion rotation, AttackEffectType type, bool isCritical = false)
    {
        PlayEffect(position, rotation, type, isCritical);
    }

    private void PlayEffect(Vector3 position, Quaternion rotation, AttackEffectType type, bool isCritical)
    {
        GameObject effect = PoolManager.instance.GetObject(PoolMapping(type, isCritical), position, rotation);
        effect.GetComponent<ParticleSystem>().Play();
    }

    public static PoolType PoolMapping(AttackEffectType type, bool isCritical)
    {
        switch (type)
        {
            case AttackEffectType.Basic:
                return !isCritical ? PoolType.VFX_BasicHit1 : PoolType.VFX_BasicHit1_Crit;
            case AttackEffectType.Basic2:
                return !isCritical ? PoolType.VFX_BasicHit2 : PoolType.VFX_BasicHit2_Crit;
            case AttackEffectType.Basic3:
                return !isCritical ? PoolType.VFX_BasicHit3 : PoolType.VFX_BasicHit3_Crit;
            case AttackEffectType.Explosion:
                return !isCritical ? PoolType.VFX_Explosion : PoolType.VFX_Explosion_Crit;
            case AttackEffectType.Ice:
                return !isCritical ? PoolType.VFX_IceHit : PoolType.VFX_IceHit_Crit;
            case AttackEffectType.Lightning:
                return !isCritical ? PoolType.VFX_LightningHit : PoolType.VFX_LightningHit_Crit;
            case AttackEffectType.Magic:
                return !isCritical ? PoolType.VFX_MagicHit : PoolType.VFX_MagicHit_Crit;
            default:
                return PoolType.VFX_BasicHit1;
        }
    }
}

public class AttackEffectSystem : MonoBehaviour
{
    private ParticleSystem particle;
    [SerializeField] private AttackEffectType effectType;
    [SerializeField] private bool isCriticalEffect;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleSystemStopped()
    {
        PoolManager.instance.ReturnObject(AttackEffect.PoolMapping(effectType, isCriticalEffect), gameObject);
    }
}
