using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FieldItemBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.5f;  // player보다 빨라야 함
    [SerializeField] private float acceleration = 1.0f;
    [SerializeField] private PoolType poolType;

    protected GameObject player;
    protected PlayerController playerController;
    private bool isAttracted = false;
    private Coroutine attractedCoroutine = null;

    private void Start()
    {
        player = GameManager.instance.Player;
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemAction();

            // Destroy the gem
            isAttracted = false;
            if (attractedCoroutine != null)
            {
                StopCoroutine(attractedCoroutine);
            }
            PoolManager.instance.ReturnObject(poolType, gameObject);
        }
        if (!isAttracted && other.CompareTag("Magnet"))
        {
            Attract();
        }
    }

    protected abstract void ItemAction();

    public void Attract()
    {
        isAttracted = true;
        attractedCoroutine = StartCoroutine(Attracted());
    }

    // 플레이어에게 빨려들어감
    private IEnumerator Attracted()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            moveSpeed += acceleration * Time.deltaTime;
            yield return null;
        }
    }

    // TODO : exp에 따라 mesh 변경 // color만 변경하면 될 것 같기도
    private void MeshChange()
    {

    }
}
