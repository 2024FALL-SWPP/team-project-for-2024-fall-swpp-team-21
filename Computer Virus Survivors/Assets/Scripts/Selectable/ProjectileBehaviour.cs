using System;
using UnityEngine;

public abstract class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    protected FinalWeaponData finalWeaponData;

    public virtual void Initialize(FinalWeaponData finalWeaponData)
    {
        this.finalWeaponData = finalWeaponData;
    }

    protected abstract void OnTriggerEnter(Collider other);
}
