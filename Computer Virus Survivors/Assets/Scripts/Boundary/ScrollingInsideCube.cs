using UnityEngine;

public class ScrollingInsideCube : MonoBehaviour
{
    public GameObject[] scrollObjects = new GameObject[4];  // 4개의 오브젝트 배열
    public float scrollSpeed = 2.0f;
    public float resetHeight = 10.0f;

    private float objectHeight;  // 인접한 오브젝트 간의 간격
    private Vector3[] startPositions = new Vector3[4];

    void Start()
    {
        // 모든 오브젝트의 초기 위치 저장
        for (int i = 0; i < scrollObjects.Length; i++)
        {
            startPositions[i] = scrollObjects[i].transform.position;
        }
        
        // 인접한 오브젝트 간의 간격 계산
        objectHeight = Mathf.Abs(startPositions[1].y - startPositions[0].y);
    }

    void Update()
    {
        // 모든 오브젝트를 아래로 스크롤
        for (int i = 0; i < scrollObjects.Length; i++)
        {
            scrollObjects[i].transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

            // 경계를 벗어났는지 확인
            if (scrollObjects[i].transform.position.y < -resetHeight)
            {
                // 가장 위에 있는 오브젝트 찾기
                float highestY = float.MinValue;
                int highestIndex = 0;
                
                for (int j = 0; j < scrollObjects.Length; j++)
                {
                    if (scrollObjects[j].transform.position.y > highestY)
                    {
                        highestY = scrollObjects[j].transform.position.y;
                        highestIndex = j;
                    }
                }

                // 현재 가장 위에 있는 오브젝트의 위로 재배치
                Vector3 newPos = scrollObjects[highestIndex].transform.position;
                newPos.y += objectHeight;
                scrollObjects[i].transform.position = newPos;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position;
        Vector3 size = new Vector3(1, resetHeight * 2, 1);
        Gizmos.DrawWireCube(center, size);
    }
}