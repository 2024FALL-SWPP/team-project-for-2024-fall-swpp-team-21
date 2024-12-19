using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransparentWall : MonoBehaviour
{
    [SerializeField] private Renderer wallRenderer;
    [SerializeField] private float transparency;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Material[] materials = wallRenderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                Color color = materials[i].color;
                color.a = transparency;
                materials[i].color = color;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Material[] materials = wallRenderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                Color color = materials[i].color;
                color.a = 1.0f;
                materials[i].color = color;
            }
        }
    }
}
