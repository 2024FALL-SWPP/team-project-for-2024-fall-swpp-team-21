using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cst = GameConstants;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameManager.instance.Player;
        transform.position = player.transform.position + Cst.CameraOffset;
        transform.LookAt(player.transform);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = player.transform.position + Cst.CameraOffset;
    }
}
