using UnityEngine;

public class UIFollowMouse : MonoBehaviour
{
    public RectTransform uiElement; // UI 요소의 RectTransform
    public float sensitivity = 0.1f; // 마우스 이동에 따른 UI 이동 비율

    private Vector3 startPosition;  // UI 요소의 초기 위치
    private Vector3 screenCenter;  // 화면 중앙 좌표

    void Start()
    {
        if (uiElement == null)
        {
            Debug.LogError("UI Element (RectTransform) is not assigned.");
            return;
        }

        // UI 초기 위치 저장
        startPosition = uiElement.localPosition;

        // 화면 중앙 좌표 계산
        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
    }

    void Update()
    {
        if (uiElement == null) return;

        // 현재 마우스 위치 가져오기
        Vector3 currentMousePosition = Input.mousePosition;

        // 마우스 위치와 화면 중앙의 차이를 계산
        Vector3 mouseDeltaFromCenter = currentMousePosition - screenCenter;

        // UI 요소의 위치 업데이트 (초기 위치를 기준으로 화면 중앙 기준 이동량 반영)
        uiElement.localPosition = startPosition + mouseDeltaFromCenter * sensitivity;
    }
}