using System;
using UnityEngine;

public abstract class PlayerProjectileBehaviour : ProjectileBehaviour
{
    [SerializeField] protected Animator animator;
    protected FinalWeaponData finalWeaponData;

    public virtual void Initialize(FinalWeaponData finalWeaponData)
    {
        if (this.finalWeaponData == null)
        {
            this.finalWeaponData = finalWeaponData;
        }
    }
}
