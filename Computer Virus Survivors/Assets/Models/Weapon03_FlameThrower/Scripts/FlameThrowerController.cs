//본 스크립트는 애니메이션이 정상 작동하는지 확인하기 위해 임시로 만든 스크립트입니다. 실제 사용시 삭제하시면 됩니다. 화염방사기 발사 애니메이션 파라미터는 "Fire_b" 입니다.s
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