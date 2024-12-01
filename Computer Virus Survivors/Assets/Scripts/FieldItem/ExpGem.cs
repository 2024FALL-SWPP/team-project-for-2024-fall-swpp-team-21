using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExpGem : FieldItemBehaviour
{
    private int exp;

    public void Initialize(int exp)
    {
        // Set the amount of exp to drop
        this.exp = exp;
        MeshChange();
    }

    protected override void ItemAction()
    {
        // Add exp to player
        playerController.GetExp(exp);
    }

    // TODO : exp에 따라 mesh 변경 // color만 변경하면 될 것 같기도
    private void MeshChange()
    {

    }
}
