using UnityEngine;

public class UIFollowMouse : MonoBehaviour
{
    public RectTransform uiElement; // UI 요소의 RectTransform
    public Vector3 startPosition;  // UI 요소의 초기 위치
    public float sensitivity = 0.1f; // 마우스 이동에 따른 UI 이동 비율

    private Vector3 previousMousePosition;

    void Start()
    {
        if (uiElement == null)
        {
            Debug.LogError("UI Element (RectTransform) is not assigned.");
            return;
        }

        // UI 초기 위치 설정
        startPosition = uiElement.localPosition;
        previousMousePosition = Input.mousePosition;
    }

    void Update()
    {
        if (uiElement == null) return;

        // 현재 마우스 위치 가져오기
        Vector3 currentMousePosition = Input.mousePosition;

        // 마우스 이동량 계산
        Vector3 mouseDelta = currentMousePosition - previousMousePosition;

        // UI 요소 위치 업데이트
        Vector3 newPosition = uiElement.localPosition + mouseDelta * sensitivity;
        uiElement.localPosition = newPosition;

        // 현재 마우스 위치를 이전 위치로 저장
        previousMousePosition = currentMousePosition;
    }
}