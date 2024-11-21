using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Piece : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Stage1Goal.instance.OnPieceGet();
            Destroy(gameObject);
        }
    }
}
