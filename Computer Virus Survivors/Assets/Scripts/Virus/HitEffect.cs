using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 피격 시 이펙트를 재생하는 클래스
/// 현재는 바이러스에만 사용되고 있음
///
/// 사용법
/// 1. 피격 이펙트를 넣고싶은 오브젝트에 HitEffect를 생성
/// 1-1. [생성자] gameObject : 피격 이펙트를 넣고싶은 오브젝트 (렌더러 컴포넌트를 가지고 있어야 함 + 코루틴의 대리자로 MonoBehaviour를 사용하기 때문)
/// 1-2. [생성자] hitMaterial : 피격시 적용할 머터리얼 (null이면 기본값으로 unlit 하얀색 머터리얼이 적용됨)
/// 2. 이펙트를 시전하고 싶은 시점에 Play() 호출
///
/// 주의사항
/// - Play() 호출 후에는 RestoreMaterials()를 호출하지 않아도 자동으로 복구도록 설계하였음
/// - 하지만 풀링 등으로 오브젝트를 재활용할 때는 RestoreMaterials()를 호출해주어야 함 <- 피격 이펙트가 실행중일 때 오브젝트가 비활성화되면 머터리얼이 복구되지 않음
/// - 따라서 바이러스의 OnDie 이벤트에서 RestoreMaterials()를 호출하도록 구현되어 있음
///
/// 추후 개선사항
/// - 소리 이펙트 추가
/// </summary>
public class HitEffect
{
    private static float _effectFrame = 4f;
    private static Material _hitMaterial;
    private MonoBehaviour ownerMonoBehaviour;
    private Renderer[] renderers;
    private Dictionary<Renderer, Material[]> originalMaterials;
    private Dictionary<Renderer, Material[]> hitMaterials;

    private static Material defaultHitMaterial
    {
        get
        {
            if (_hitMaterial == null)
            {
                _hitMaterial = new Material(Shader.Find("Unlit/Color"));
                _hitMaterial.color = Color.white;
            }
            return _hitMaterial;
        }
    }

    public HitEffect(GameObject gameObject, Material hitMaterial = null)
    {
        ownerMonoBehaviour = gameObject.GetComponent<MonoBehaviour>();

        renderers = gameObject.GetComponentsInChildren<Renderer>();
        originalMaterials = new Dictionary<Renderer, Material[]>();
        foreach (Renderer renderer in renderers)
        {
            originalMaterials.Add(renderer, renderer.materials);
        }

        hitMaterials = new Dictionary<Renderer, Material[]>();
        foreach (Renderer renderer in renderers)
        {
            hitMaterials.Add(renderer, new Material[renderer.materials.Length]);
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                hitMaterials[renderer][i] = hitMaterial == null ? defaultHitMaterial : hitMaterial;
            }
        }
    }

    public void Play()
    {
        if (ownerMonoBehaviour is VirusBehaviour)
        {
            VirusBehaviour virus = ownerMonoBehaviour.gameObject.GetComponent<VirusBehaviour>();
            virus.OnDie += (_) =>
            {
                RestoreMaterials();
            };
        }
        ownerMonoBehaviour.StartCoroutine(MaterialChange());
    }

    private IEnumerator MaterialChange()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.materials = hitMaterials[renderer];
        }

        Debug.Log("HitEffect : wait for " + _effectFrame / 60f + " seconds");
        yield return new WaitForSeconds(_effectFrame / 60f);

        RestoreMaterials();
    }

    private void RestoreMaterials()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.materials = originalMaterials[renderer];
        }
    }
}
