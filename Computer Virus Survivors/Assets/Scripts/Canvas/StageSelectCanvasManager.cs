using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectCanvasManager : CanvasBase
{

    [SerializeField] private List<GameObject> stages;
    [SerializeField] private List<String> stageNames;
    [SerializeField] private RectTransform stageSelectedHighlight;
    [SerializeField] private Button gameStartButton;

    public event Action GotoPreviousHandler;
    public event Action<String> StageSelectedHandler;

    private int selectedStageIndex;

    public override void Initialize()
    {
        selectedStageIndex = 0;
        gameObject.SetActive(false);
    }

    public void OnGotoPreviousClicked()
    {
        GotoPreviousHandler?.Invoke();
    }

    public void OnStageClicked(int index)
    {
        selectedStageIndex = index;
        if (stageNames[selectedStageIndex] != null && stageNames[selectedStageIndex] != "")
        {
            gameStartButton.interactable = true;
        }
        else
        {
            gameStartButton.interactable = false;
        }
    }

    public void OnGameStartClicked()
    {
        StageSelectedHandler?.Invoke(stageNames[selectedStageIndex]);
    }

    private void Update()
    {
        stageSelectedHighlight.position = Vector2.Lerp(stageSelectedHighlight.position, stages[selectedStageIndex].transform.position, Time.deltaTime * 10f);
    }

    public override void OnEnter()
    {
        selectedStageIndex = 0;
        gameStartButton.interactable = true;
    }

    public override void OnExit()
    {
    }

}

