using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(0, 10, -10);

    // Start is called before the first frame update
    // private void Start()
    // {

    // }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
    }
}
