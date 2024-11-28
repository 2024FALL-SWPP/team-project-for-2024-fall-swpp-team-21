using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneCanvasManager : Singleton<MainSceneCanvasManager>
{

    [SerializeField] private readonly MainSceneCanvas mainSceneCanvas;
    [SerializeField] private readonly CharacterSelectCanvas characterSelectCanvas;
    // [SerializeField] private StageSelectCanvas stageSelectCanvas;

    private IState mainSceneState;
    private IState characterSelectState;
    private IState stageSelectState;
    private IState playingState;
    private IState currentState;

    private PlayerStatData selectedPlayer;
    private string selectedStageName;

    public override void Initialize()
    {
        mainSceneCanvas.Initialize();
        characterSelectCanvas.Initialize();
        mainSceneCanvas.BtnCliked += (index) =>
        {
            if (index == 0)
            {
                StateMachine(Signal.OnGameStartClicked);
            }
            else if (index == 1)
            {
                StateMachine(Signal.OnExitClicked);
            }
        };
        characterSelectCanvas.GotoPreviousHandler += () =>
        {
            StateMachine(Signal.OnGotoPreviousClicked);
        };

        // TODO : characterSelectState, stageSelectState 초기화
        mainSceneState = mainSceneCanvas;
        currentState = mainSceneState;
        characterSelectState = characterSelectCanvas;
    }


    private enum Signal
    {
        OnGameStartClicked,
        OnExitClicked,
        OnCharacterSelectDone,
        OnStageSelectDone,
        OnGotoPreviousClicked,
        OnGameCleared,
    }

    private void StateMachine(Signal signal)
    {
        void SetState(IState state)
        {
            currentState.OnExit();
            currentState = state;
            currentState.OnEnter();
        }

        if (currentState == mainSceneState)
        {
            switch (signal)
            {
                case Signal.OnGameStartClicked:
                    characterSelectCanvas.gameObject.SetActive(true);
                    SetState(characterSelectState);
                    break;
                case Signal.OnExitClicked:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    break;
            }
        }
        else if (currentState == characterSelectState)
        {
            switch (signal)
            {
                case Signal.OnCharacterSelectDone:
                    // stageSelectCanvas.gameObject.SetActive(true);
                    SetState(stageSelectState);
                    break;
                case Signal.OnGotoPreviousClicked:
                    mainSceneCanvas.gameObject.SetActive(true);
                    SetState(mainSceneState);
                    break;
            }
        }
        else if (currentState == stageSelectState)
        {
            switch (signal)
            {
                case Signal.OnStageSelectDone:
                    SetState(playingState);
                    break;
                case Signal.OnGotoPreviousClicked:
                    characterSelectCanvas.gameObject.SetActive(true);
                    SetState(characterSelectState);
                    break;
            }
        }
        else if (currentState == playingState)
        {
            switch (signal)
            {
                case Signal.OnGameCleared:
                    SetState(mainSceneState);
                    break;
            }
        }
    }

    public string GetSelectedStageName()
    {
        return selectedStageName;
    }

    private class PlayingState : IState
    {
        public void OnEnter()
        {
            // fade out

            // 씬 전환
            SceneManager.LoadScene(MainSceneCanvasManager.instance.GetSelectedStageName());

        }

        public void OnExit()
        {
            ;
        }
    }
}

public class MainSceneCanvas : Singleton<MainSceneCanvas>, IState
{

    public event Action<int> BtnCliked;

    public override void Initialize()
    {
        // TODO : 초기화
        // 배경음악 재생
    }

    public void OnGameStartClicked()
    {
        BtnCliked?.Invoke(0);
    }

    public void OnExitClicked()
    {
        BtnCliked?.Invoke(1);
    }

    public void OnEnter()
    {
        // TODO : 메인화면 진입하면 하는것? 애니메이션 정도?
        ;
    }

    public void OnExit()
    {
        gameObject.SetActive(false);
    }
}
