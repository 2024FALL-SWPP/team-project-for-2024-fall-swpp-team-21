using UnityEngine;

public class V_Virus01SmallLightController : MonoBehaviour
{
    public GameObject[] targetObjects; // 라이트 효과를 줄 오브젝트들
    private Light pointLight;

    void Start()
    {
        pointLight = GetComponent<Light>();
        pointLight.cullingMask = 0; // 컬링 마스크를 Nothing으로 설정하여 모든 오브젝트에 빛을 비추지 않도록 설정

        ApplyLightEffectToTargets();
    }

    void ApplyLightEffectToTargets()
    {
        foreach (GameObject target in targetObjects)
        {
            // 오브젝트의 메터리얼에서 Emission을 활성화하여 빛을 받는 효과를 추가
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", pointLight.color * pointLight.intensity);
            }
        }
    }

    void OnDisable()
    {
        foreach (GameObject target in targetObjects)
        {
            // 스크립트가 비활성화될 때 Emission을 비활성화하여 원래 상태로 복구
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.DisableKeyword("_EMISSION");
            }
        }
    }
}