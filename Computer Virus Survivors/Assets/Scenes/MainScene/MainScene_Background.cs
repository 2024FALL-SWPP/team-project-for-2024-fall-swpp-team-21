using UnityEngine;
using UnityEngine.UI;

public class MainScene_Background : MonoBehaviour
{
    public Image gradientImage; // 검은색-투명 그래디언트 이미지

    void Update()
    {
        // 화면 너비와 마우스 X 위치 가져오기
        float mouseX = Input.mousePosition.x;
        float screenWidth = Screen.width;

        // 밝기 계산 (0: 완전 어두움, 1: 투명)
        float alpha = Mathf.Clamp01(1 - (mouseX / screenWidth));

        // 그래디언트 이미지의 색상 업데이트 (투명도만 조정)
        Color currentColor = gradientImage.color;
        gradientImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
    }
}