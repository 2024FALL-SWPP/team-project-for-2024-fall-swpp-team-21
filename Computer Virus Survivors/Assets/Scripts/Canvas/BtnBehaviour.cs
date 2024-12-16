using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private BtnSoundPreset btnSoundPreset;

    private Button button;
    private bool isMouseOver = false;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (IsMouseOverUIElement())
        {
            isMouseOver = true;
        }
        else
        {
            isMouseOver = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isMouseOver)
        {
            BtnSoundManager.instance.PlaySound(btnSoundPreset.MouseEnter);
            isMouseOver = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BtnSoundManager.instance.PlaySound(btnSoundPreset.MouseExit);
        isMouseOver = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button.interactable == true)
        {
            BtnSoundManager.instance.PlaySound(btnSoundPreset.ButtonClick);
        }
        else
        {
            BtnSoundManager.instance.PlaySound(btnSoundPreset.ButtonClickFail);
        }
    }

    private bool IsMouseOverUIElement()
    {
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(button.transform as RectTransform, Input.mousePosition, null, out localMousePosition);

        return (button.transform as RectTransform).rect.Contains(localMousePosition);
    }

}
