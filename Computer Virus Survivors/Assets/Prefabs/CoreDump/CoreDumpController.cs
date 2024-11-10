using UnityEngine;

public class CoreDumpController : MonoBehaviour
{
    public Animator animator;
    private bool isGrounded = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 스페이스바 입력을 감지하여 충돌 상태로 전환
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = true;
            animator.SetBool("isGrounded_b", isGrounded);
        }
    }
}