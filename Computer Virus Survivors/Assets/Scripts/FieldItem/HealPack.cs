using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HealPack : FieldItemBehaviour
{
    [SerializeField] private int healAmount;

    protected override void ItemAction()
    {
        // Heal
        playerController.GetHeal(healAmount);
    }
}
