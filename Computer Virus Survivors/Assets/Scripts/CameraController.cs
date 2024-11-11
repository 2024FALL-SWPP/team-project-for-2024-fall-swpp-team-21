using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cst = GameConstants;

public class CameraController : Singleton<CameraController>
{
    private GameObject player;

    public override void Initialize()
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
