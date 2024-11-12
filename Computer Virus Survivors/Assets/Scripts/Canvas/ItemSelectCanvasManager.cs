using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectCanvasManager : Singleton<ItemSelectCanvasManager>, IState
{

    public event Action<SelectableBehaviour> SelectionHandler;

    [SerializeField] private List<GameObject> itemSelectBtn;

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
                itemSelectBtn[i].SetActive(false);
                continue;
            }
            itemSelectBtn[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = choices[i].ToString();
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
            ShowItemSelectCanvas();
            isShowing = true;
        }
    }

    public void OnExit()
    {
    }

}
