using UnityEngine;
using TMPro;

public class TextFollowMouse : MonoBehaviour
{
    public TextMeshProUGUI textObject; // TextMeshPro 오브젝트
    public Vector3 startPosition;     // 텍스트의 초기 위치
    public float sensitivity = 0.1f;  // 마우스 이동에 따른 텍스트 이동 비율 (10분의 1)

    private Vector3 previousMousePosition;

    void Start()
    {
        if (textObject == null)
        {
            Debug.LogError("TextMeshProUGUI object is not assigned.");
            return;
        }

        // 텍스트 초기 위치 설정
        startPosition = textObject.rectTransform.localPosition;
        previousMousePosition = Input.mousePosition;
    }

    void Update()
    {
        // 현재 마우스 위치 가져오기
        Vector3 currentMousePosition = Input.mousePosition;

        // 마우스 이동량 계산
        Vector3 mouseDelta = currentMousePosition - previousMousePosition;

        // 텍스트 위치 업데이트 (10분의 1만 이동)
        Vector3 newPosition = textObject.rectTransform.localPosition + mouseDelta * sensitivity;
        textObject.rectTransform.localPosition = newPosition;

        // 현재 마우스 위치를 이전 위치로 저장
        previousMousePosition = currentMousePosition;
    }
}