using UnityEngine;
using TMPro;

public class TextHoverEffect : MonoBehaviour
{
    public TextMeshProUGUI textObject; // TextMeshPro 텍스트
    public Color normalColor = Color.white; // 기본 색상
    public Color hoverColor = Color.yellow; // 마우스 오버 색상

    private void Start()
    {
        // 기본 색상 설정
        textObject.color = normalColor;
    }

    void Update()
    {
        // Ray 생성 (마우스 위치 기준)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Ray가 충돌했는지 확인
        if (Physics.Raycast(ray, out hit))
        {
            // 충돌한 오브젝트가 이 TMP 오브젝트인지 확인
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                textObject.color = hoverColor; // 색상 변경
                return; // Update 종료
            }
        }

        // 마우스가 벗어나면 기본 색상으로 복구
        textObject.color = normalColor;
    }
}