using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public interface IState
{
    void OnEnter();
    void OnExit();
}

public class CanvasManager : Singleton<CanvasManager>, IPlayerStatObserver
{

    [SerializeField] private PlayerStatEventCaller playerStatEventCaller;

    [SerializeField] private ItemSelectCanvasManager itemSelectCanvas;
    [SerializeField] private PauseCanvas pauseCanvas;
    [SerializeField] private PlayerGUI playerGUI;

    private ReadyState readyState;
    private IState playingState;
    private IState itemSelectState;
    private IState pausedState;
    private IState gameOverState;
    private IState currentState;
    private bool isItemSelecting;

    public override void Initialize()
    {
        readyState = new GameObject("ReadyState").AddComponent<ReadyState>();
        readyState.Initialize();
        itemSelectCanvas.Initialize();
        pauseCanvas.Initialize();
        playerGUI.Initialize();

        playerStatEventCaller.StatChangedHandler += OnStatChanged;
        itemSelectCanvas.SelectionHandler += (selectableBehaviour) =>
        {
            StateMachine(Signal.OnItemSelectDone);
        };
        pauseCanvas.ResumeHandler += () =>
        {
            StateMachine(Signal.OnResumeClicked);
        };

        playingState = new PlayingState();
        itemSelectState = itemSelectCanvas;
        pausedState = pauseCanvas;
        // TODO : gameOverState 초기화
        currentState = readyState;
        isItemSelecting = false;
    }

    public void GameStart()
    {
        StartCoroutine(readyState.FadeIn());
        StateMachine(Signal.GameStart);
    }


    private enum Signal
    {
        GameStart,
        LevelUp,
        GameOver,
        OnPauseClicked,
        OnResumeClicked,
        OnItemSelectDone
    }

    public void OnPauseBtnClicked()
    {
        StateMachine(Signal.OnPauseClicked);
    }

    public void OnStatChanged(object sender, StatChangedEventArgs args)
    {
        if (args.StatName == nameof(PlayerStat.PlayerLevel))
        {
            if ((int) args.NewValue > 1)
            {
                StateMachine(Signal.LevelUp);
            }

        }
    }

    private void StateMachine(Signal signal)
    {
        void SetState(IState newState)
        {
            currentState.OnExit();
            Debug.Log("State Change : " + currentState + " -> " + newState);
            currentState = newState;
            currentState.OnEnter();
        }

        if (currentState == (IState) readyState)
        {
            switch (signal)
            {
                case Signal.GameStart:
                    SetState(playingState);
                    break;
            }
        }
        else if (currentState == playingState)
        {
            switch (signal)
            {
                case Signal.LevelUp:
                    itemSelectCanvas.gameObject.SetActive(true);
                    isItemSelecting = true;
                    SetState(itemSelectState);
                    break;
                // case Signal.GameOver:
                //     SetState(gameOverState);
                //     break;
                case Signal.OnPauseClicked:
                    pauseCanvas.gameObject.SetActive(true);
                    SetState(pausedState);
                    break;
                    // default:
                    //     throw new Exception("Unresolved Signal : " + nameof(signal));
            }
        }
        else if (currentState == pausedState)
        {
            switch (signal)
            {
                case Signal.OnResumeClicked:
                    pauseCanvas.gameObject.SetActive(false);
                    if (isItemSelecting)
                    {
                        SetState(itemSelectState);
                    }
                    else
                    {
                        SetState(playingState);
                    }
                    break;
                    // default:
                    //     throw new Exception("Unresolved Signal : " + nameof(signal));
            }
        }
        else if (currentState == itemSelectState)
        {
            switch (signal)
            {
                case Signal.OnItemSelectDone:
                    itemSelectCanvas.gameObject.SetActive(false);
                    isItemSelecting = false;
                    SetState(playingState);
                    break;
                case Signal.OnPauseClicked:
                    pauseCanvas.gameObject.SetActive(true);
                    SetState(pausedState);
                    break;
                    // default:
                    //     throw new Exception("Unresolved Signal : " + nameof(signal));
            }
        }
        // else if (currentState == gameOverState)
        // {
        //     // TODO : GameOverState에서의 Signal 처리
        // }
    }

    private class PlayingState : IState
    {
        public void OnEnter()
        {
            Time.timeScale = 1;
        }

        public void OnExit()
        {
            Time.timeScale = 0;
        }

    }

    private class ReadyState : MonoBehaviour, IState
    {

        private GameObject fadeImage;

        private float fadeTime = 1f;

        public void Initialize()
        {
            fadeImage = new GameObject("FadeImage", typeof(Image));
            fadeImage.transform.SetParent(CanvasManager.instance.transform);
            RectTransform rect = fadeImage.GetComponent<RectTransform>();
            rect.anchorMax = new Vector2(1, 1);
            rect.anchorMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
            rect.offsetMin = new Vector2(0, 0);
            Image image = fadeImage.GetComponent<Image>();
            image.color = new Color(0, 0, 0, 1);
            fadeImage.transform.SetAsFirstSibling();
            gameObject.SetActive(true);
        }

        public IEnumerator FadeIn()
        {
            float elpasedTime = 0;
            Image image = fadeImage.GetComponent<Image>();
            while (elpasedTime < fadeTime)
            {
                elpasedTime += Time.deltaTime;
                image.color = new Color(0, 0, 0, 1 - elpasedTime);
                yield return null;
            }
        }

        public void OnEnter()
        {

        }

        public void OnExit()
        {

        }
    }
}


