using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> itemSelectBtn;

    private List<SelectionInfo> choices;
    private PlayerStat playerStat;

    private void OnEnable()
    {
        if (playerStat == null)
        {
            playerStat = player.GetComponent<PlayerController>().playerStat;
        }
        ShowItemSelectCanvas();
    }

    public void ShowItemSelectCanvas()
    {
        Debug.Log("ShowItemSelectCanvas");
        choices = SelectableManager.instance.GetChoices(playerStat.GetPlayerWeaponInfos(), playerStat.GetPlayerItemInfos());

        if (choices.Count == 0)
        {
            return;
        }

        for (int i = 0; i < choices.Count; i++)
        {
            itemSelectBtn[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = choices[i].ToString();
        }
    }

    public void OnClick(int selectedIndex)
    {
        playerStat.AddSelectable(choices[selectedIndex].selectableBehaviour);

        CanvasManager.instance.OnSelectionDone();
    }

}
