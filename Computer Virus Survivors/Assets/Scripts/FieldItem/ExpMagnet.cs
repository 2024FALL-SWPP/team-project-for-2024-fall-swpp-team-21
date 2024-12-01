using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExpMagnet : FieldItemBehaviour
{
    [SerializeField] private LayerMask layerMask;

    protected override void ItemAction()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("ExpGem");
        foreach (GameObject obj in taggedObjects)
        {
            obj.GetComponent<FieldItemBehaviour>().Attract();
        }
    }
}
