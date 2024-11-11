using UnityEngine;

public class FlameThrowerController : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스 키로 발사 제어
        {
            animator.SetBool("Fire_b", true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("Fire_b", false);
        }
    }
}