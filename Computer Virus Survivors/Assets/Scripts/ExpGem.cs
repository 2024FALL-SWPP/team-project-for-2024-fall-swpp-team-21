using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    private int exp;
    private bool inGainRange = false;
    private GameObject player;
    public float moveSpeed = 5.5f;  // player보다 빨라야 함

    public void Initialize(int exp)
    {
        // Set the amount of exp to drop
        this.exp = exp;
    }

    private void Update()
    {
        if (inGainRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
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
        if (other.CompareTag("Magnet"))
        {
            player = other.gameObject.transform.parent.gameObject;
            inGainRange = true;
        }
    }

    // TODO : exp에 따라 mesh 변경 // color만 변경하면 될 것 같기도
    private void MeshChange()
    {

    }
}
