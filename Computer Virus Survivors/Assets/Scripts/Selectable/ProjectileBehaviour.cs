using System;
using UnityEngine;

public abstract class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    protected int damage;

    public virtual void Initialize(int damage)
    {
        this.damage = damage;
    }

    protected abstract void OnTriggerEnter(Collider other);

    protected bool CheckOutOfScreen()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1;
    }
}
