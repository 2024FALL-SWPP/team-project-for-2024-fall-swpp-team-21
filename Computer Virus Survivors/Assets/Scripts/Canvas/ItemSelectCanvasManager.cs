using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectCanvasManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemSelectBtn;

    private List<SelectionInfo> choices;
    private PlayerStat playerStat;

    private void OnEnable()
    {
        if (playerStat == null)
        {
            playerStat = GameManager.instance.Player.GetComponent<PlayerController>().playerStat;
        }
        ShowItemSelectCanvas();
    }

    public void ShowItemSelectCanvas()
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
        playerStat.TakeSelectable(SelectableManager.instance.GetSelectableBehaviour(choices[selectedIndex].objectName));

        CanvasManager.instance.OnSelectionDone();
    }

}
