using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageIndicator : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    [SerializeField] private float displayDuration = 1f;
    [SerializeField] private float moveupSpeed = 0.3f;
    [SerializeField] private static Vector3 _offset = new Vector3(0, 2, 0);
    [SerializeField] private Material[] materials = new Material[2];

    private Vector3 worldPosition;
    private RectTransform rectTransform;
    private float originalFontSize;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        originalFontSize = textMesh.fontSize;
    }

    private void OnEnable()
    {
        rectTransform.localScale = Vector3.one;
    }

    public void Initialize(int damage, Vector3 virusPosition, bool isCritical = false)
    {
        worldPosition = virusPosition + _offset;
        SetDamage(damage, isCritical);
        transform.SetParent(CanvasManager.instance.damageIndicators.transform);
    }

    private void SetDamage(int damage, bool isCritical)
    {
        textMesh.text = damage.ToString();
        textMesh.fontMaterial = isCritical ? materials[1] : materials[0];
        StartCoroutine(DisplayDamage());
    }

    private IEnumerator DisplayDamage()
    {
        yield return new WaitForSeconds(displayDuration / 2f);

        float elapsedTime = 0;
        while (elapsedTime < displayDuration / 2f)
        {
            elapsedTime += Time.deltaTime;
            textMesh.fontSize = Mathf.Lerp(originalFontSize, 0, elapsedTime / (displayDuration / 2f));
            yield return null;
        }
        PoolManager.instance.ReturnObject(PoolType.DamageIndicator, gameObject);

    }

    private void LateUpdate()
    {
        worldPosition += Vector3.up * moveupSpeed * Time.deltaTime;
        //transform.position = Camera.main.WorldToScreenPoint(worldPosition);
        rectTransform.position = Camera.main.WorldToScreenPoint(worldPosition);
    }

    // private void LateUpdate()
    // {
    //     transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
    //                  Camera.main.transform.rotation * Vector3.up);
    // }

}
