using UnityEngine;

public class W_PacketStream_Anim : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 50, 0); // 초당 회전 속도 (각도 단위)

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}