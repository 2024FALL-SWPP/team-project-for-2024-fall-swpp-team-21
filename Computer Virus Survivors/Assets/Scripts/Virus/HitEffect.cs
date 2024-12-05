using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect
{
    private static float _effectFrame = 4f;
    private static Material _hitMaterial;
    private MonoBehaviour ownerMonoBehaviour;
    private Renderer[] renderers;
    private Dictionary<Renderer, Material[]> originalMaterials;
    private Dictionary<Renderer, Material[]> hitMaterials;

    private static Material hitMaterial
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

    public HitEffect(GameObject gameObject)
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
                hitMaterials[renderer][i] = hitMaterial;
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
