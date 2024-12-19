using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExpGem : FieldItemBehaviour
{
    [SerializeField] private Renderer cubeRenderer;

    [Header("색상 범위 설정용 변수")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private int startExp;
    [SerializeField] private int endExp;
    [SerializeField] private float intensity = 1.0f;

    private int exp;

    public void Initialize(int exp)
    {
        // Set the amount of exp to drop
        this.exp = exp;
        MeshChange();
    }

    protected override void ItemAction()
    {
        // Add exp to player
        getSFX?.Play();
        playerController.GetExp(exp);
    }

    // exp에 따라 mesh 변경
    private void MeshChange()
    {
        float invLerp = Mathf.InverseLerp(startExp, endExp, exp);
        cubeRenderer.material.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, invLerp) * intensity);
    }
}
