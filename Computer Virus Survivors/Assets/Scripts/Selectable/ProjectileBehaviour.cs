using System;
using UnityEngine;

public abstract class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    protected int damage;

    public void Initialize(int damage)
    {
        this.damage = damage;
    }

    protected abstract void OnTriggerEnter(Collider other);
}
