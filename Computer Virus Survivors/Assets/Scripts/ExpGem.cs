using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    private int exp;
    private bool isAttracted = false;
    private GameObject player;
    public float moveSpeed = 5.5f;  // player보다 빨라야 함

    public void Initialize(int exp)
    {
        // Set the amount of exp to drop
        this.exp = exp;
        // Player도 그냥 처음에 갖고 시작하는게 나아보임
        // this.player = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add exp to player
            other.GetComponent<PlayerController>().GetExp(exp);
            // Destroy the gem
            isAttracted = false;
            gameObject.SetActive(false);
        }
        if (!isAttracted && other.CompareTag("Magnet"))
        {
            player = other.gameObject.transform.parent.gameObject;
            isAttracted = true;
            StartCoroutine(Attracted());
        }
    }

    // 플레이어에게 빨려들어감
    private IEnumerator Attracted()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // TODO : exp에 따라 mesh 변경 // color만 변경하면 될 것 같기도
    private void MeshChange()
    {

    }
}
