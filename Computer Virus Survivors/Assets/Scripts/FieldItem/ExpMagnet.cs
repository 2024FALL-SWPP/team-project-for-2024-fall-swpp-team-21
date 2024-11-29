using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExpMagnet : FieldItemBehaviour
{
    [SerializeField] private float magnetRadius = 10.0f;
    [SerializeField] private LayerMask layerMask;

    protected override void ItemAction()
    {
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, magnetRadius, layerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("ExpGem"))
            {
                collider.GetComponent<ExpGem>().Attract();
            }
        }
    }
}
