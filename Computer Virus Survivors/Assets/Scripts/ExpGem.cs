using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    private int exp;

    public void Initialize(int exp)
    {
        // Set the amount of exp to drop
        this.exp = exp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add exp to player
            other.GetComponent<PlayerController>().GetExp(exp);
            // Destroy the gem
            gameObject.SetActive(false);
        }
    }

    // TODO : exp에 따라 mesh 변경 // color만 변경하면 될 것 같기도
    private void MeshChange()
    {

    }
}
