using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
public class DissolveEffect
{
    private List<Material> materials = new List<Material>();
    private GameObject ownerObject;
    private float effectTime;

    public DissolveEffect(GameObject gameObject, float effectTime = 1f)
    {
        ownerObject = gameObject;
        this.effectTime = effectTime;
        var renders = gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            materials.AddRange(renders[i].materials);
        }
    }

    public void Reset()
    {
        SetValue(0);
    }

    public void Play(Action callback = null)
    {
        SetRandomSeed();
        ownerObject.GetComponent<MonoBehaviour>().StartCoroutine(PlayDissolveEffect(callback));
    }

    private IEnumerator PlayDissolveEffect(Action callback = null)
    {
        float elapsedTime = 0;
        while (elapsedTime < effectTime)
        {
            var value = Mathf.Lerp(0, 1f, elapsedTime / effectTime);
            elapsedTime += Time.deltaTime;
            SetValue(value);
            yield return null;
        }
        SetValue(1f);
        callback?.Invoke();
    }

    private void SetRandomSeed()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetFloat("_NoiseSeed", UnityEngine.Random.Range(0, 1000));
        }
    }

    private void SetValue(float value)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetFloat("_Dissolve", value);
        }
    }

    public void SetEdgeWidth(float width)
    {
        if (width < 0 || width > 1)
        {
            throw new Exception("Edge width must be between 0 and 1");
        }

        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetFloat("_EdgeWidth", width);
        }
    }

    public void SetEdgeColor(Color color)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetColor("_EdgeColor", color);
        }
    }
}
