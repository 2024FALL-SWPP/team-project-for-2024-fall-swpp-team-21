using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>, IPlayerStatObserver, IState
{

    [SerializeField] private PlayerStatEventCaller playerStatEventCaller;
    [SerializeField] private ItemSelectCanvasManager itemSelectCanvas;
    [SerializeField] private PlayerGUI playerGUI;

    public event Action SelectionDoneHandler;
    private CanvasFSM canvasFSM;


    public override void Initialize()
    {

        playerStatEventCaller.StatChangedHandler += OnStatChanged;
        playerGUI.Initialize();

        canvasFSM = new CanvasFSM(playerStatEventCaller);
        canvasFSM.Mapping(CanvasFSM.CanvasName.Canvas, this);
        canvasFSM.Mapping(CanvasFSM.CanvasName.ItemSelect, itemSelectCanvas);
    }

    // public void OnSelectionDone()
    // {
    //     itemSelectCanvas.gameObject.SetActive(false);
    //     SelectionDoneHandler?.Invoke();
    // }



    public void OnEnter()
    {
        Time.timeScale = 1;
    }

    public void OnExit()
    {
        Time.timeScale = 0;
    }

    public void OnUpdate()
    {
    }
}
