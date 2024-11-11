using UnityEngine;

public class LightController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("LightOn_b", false); // 초기화
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("LightOn_b", true);
        }
        else
        {
            animator.SetBool("LightOn_b", false); // 스페이스바가 아닐 때 항상 false 유지
        }
    }
}