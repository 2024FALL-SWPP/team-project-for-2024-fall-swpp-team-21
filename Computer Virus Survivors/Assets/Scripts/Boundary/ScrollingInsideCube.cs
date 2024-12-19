using UnityEngine;

public class ScrollingInsideCube : MonoBehaviour
{
    [SerializeField] private GameObject scrollObject;  // 4개의 오브젝트 배열
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float resetHeight;

    private void Update()
    {
        // 모든 오브젝트를 아래로 스크롤
        scrollObject.transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // 경계를 벗어났는지 확인
        if (scrollObject.transform.position.y < -resetHeight)
        {
            // 현재 가장 위에 있는 오브젝트의 위로 재배치
            Vector3 newPos = scrollObject.transform.position;
            newPos.y = 0;
            scrollObject.transform.position = newPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position;
        Vector3 size = new Vector3(1, resetHeight * 2, 1);
        Gizmos.DrawWireCube(center, size);
    }
}