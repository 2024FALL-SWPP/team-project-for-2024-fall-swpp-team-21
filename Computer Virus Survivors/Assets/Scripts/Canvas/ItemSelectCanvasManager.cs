using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemSelectCanvasManager : Singleton<ItemSelectCanvasManager>, IState
{

    public event Action<SelectableBehaviour> SelectionHandler;

    [SerializeField] private List<ItemSelectPanel> itemSelectBtn;
    [SerializeField] private CanvasSoundPreset canvasSoundPreset;

    private List<SelectionInfo> choices;
    private bool isShowing = false;

    public override void Initialize()
    {
        gameObject.SetActive(false);
    }

    private void ShowItemSelectCanvas()
    {
        Debug.Log("ShowItemSelectCanvas");

        choices = SelectableManager.instance.GetChoices();

        for (int i = 0; i < 3; i++)
        {
            if (choices.Count <= i)
            {
                itemSelectBtn[i].itemSelectPanel.SetActive(false);
                continue;
            }
            itemSelectBtn[i].SetContents(choices[i]);
        }
    }

    public void OnClick(int selectedIndex)
    {
        Debug.Log("OnClick" + choices[selectedIndex].objectName);
        SelectionHandler?.Invoke(SelectableManager.instance.GetSelectableBehaviour(choices[selectedIndex].objectName));

        isShowing = false;
    }

    public void OnEnter()
    {
        transform.SetAsLastSibling();
        if (!isShowing)
        {
            UISoundManager.instance.PlaySound(canvasSoundPreset.EnterSound);
            ShowItemSelectCanvas();
            isShowing = true;
        }
    }

    public void OnExit()
    {
    }

    [Serializable]
    private class ItemSelectPanel
    {
        public GameObject itemSelectPanel;
        public TMPro.TextMeshProUGUI typeText;
        public TMPro.TextMeshProUGUI nameText;
        public TMPro.TextMeshProUGUI levelChangeText;
        public TMPro.TextMeshProUGUI explanationText;
        public Image icon;

        public void SetContents(SelectionInfo item)
        {
            typeText.text = item.type;
            nameText.text = item.objectName;
            levelChangeText.text = item.levelChange;
            explanationText.text = item.explanation;
            icon.sprite = item.icon;
        }
    }

}
