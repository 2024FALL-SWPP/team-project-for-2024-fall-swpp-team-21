using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>, IPlayerStatObserver
{

    [SerializeField] private PlayerStatEventCaller playerStatEventCaller;
    [SerializeField] private ItemSelectCanvasManager itemSelectCanvas;
    [SerializeField] private PlayerGUI playerGUI;

    public event Action SelectionDoneHandler;

    public override void Initialize()
    {
        playerStatEventCaller.StatChangedHandler += OnStatChanged;
        playerGUI.Initialize();
    }

    public void OnSelectionDone()
    {
        Time.timeScale = 1;
        itemSelectCanvas.gameObject.SetActive(false);
        SelectionDoneHandler?.Invoke();
    }

    public void OnStatChanged(object sender, StatChangedEventArgs args)
    {
        Debug.Log("StatChanged : " + args.StatName);
        if (args.StatName == nameof(PlayerStat.PlayerLevel))
        {
            Time.timeScale = 0;
            itemSelectCanvas.gameObject.SetActive(true);
        }
    }
}
