using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectCanvasManager : MonoBehaviour, IState
{

    public event Action<SelectableBehaviour> SelectionHandler;

    [SerializeField] private List<GameObject> itemSelectBtn;

    private List<SelectionInfo> choices;
    private PlayerStat playerStat;
    private RectTransform rectTransform;

    public void ShowItemSelectCanvas()
    {
        Debug.Log("ShowItemSelectCanvas");
        rectTransform.SetAsLastSibling();
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
        playerStat.TakeSelectable(SelectableManager.instance.GetSelectableBehaviour(choices[selectedIndex].objectName));

        CanvasManager.instance.OnSelectionDone();
    }

    public void OnEnter()
    {
        gameObject.SetActive(true);
        if (playerStat == null)
        {
            playerStat = GameManager.instance.Player.GetComponent<PlayerController>().playerStat;
            rectTransform = GetComponent<RectTransform>();
        }
        ShowItemSelectCanvas();
    }

    public void OnExit()
    {

        gameObject.SetActive(false);
    }

    public void OnUpdate()
    {
    }

}
