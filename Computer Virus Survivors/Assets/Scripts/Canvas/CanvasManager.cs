using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameEndCanvasManager gameOverCanvas;
    [SerializeField] private GameEndCanvasManager gameClearCanvas;

    [HideInInspector]
    public GameObject damageIndicators;
    private ReadyState readyState;
    private IState playingState;
    private IState itemSelectState;
    private IState pausedState;
    private IState gameOverState;
    private IState gameClearState;
    private IState currentState;
    private bool isItemSelecting;

    public override void Initialize()
    {
        damageIndicators = new GameObject("DamageIndicators", typeof(Canvas));
        damageIndicators.transform.SetParent(transform);
        RectTransform rect = damageIndicators.GetComponent<RectTransform>();
        rect.anchorMax = new Vector2(1, 1);
        rect.anchorMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
        rect.offsetMin = new Vector2(0, 0);
        rect.localScale = Vector3.one;
        damageIndicators.transform.SetAsFirstSibling();
        playerGUI.transform.SetAsFirstSibling();
        readyState = new GameObject("ReadyState").AddComponent<ReadyState>();
        readyState.Initialize();
        itemSelectCanvas.Initialize();
        pauseCanvas.Initialize();
        playerGUI.Initialize();
        gameOverCanvas.Initialize();
        gameClearCanvas.Initialize();

        playerStatEventCaller.StatChangedHandler += OnStatChanged;
        itemSelectCanvas.SelectionHandler += (selectableBehaviour) =>
        {
            StateMachine(Signal.OnItemSelectDone);
        };
        pauseCanvas.ResumeHandler += () =>
        {
            StateMachine(Signal.OnResumeClicked);
        };
        gameOverCanvas.GotoHomeBtnHandler += () =>
        {
            StateMachine(Signal.GotoMainClicked);
        };
        gameClearCanvas.GotoHomeBtnHandler += () =>
        {
            StateMachine(Signal.GotoMainClicked);
        };
        GameManager.instance.GameOverHandler += () =>
        {
            StateMachine(Signal.GameOver);
        };
        GameManager.instance.GameClearHandler += () =>
        {
            StateMachine(Signal.GameClear);
        };
        GameManager.instance.GotoMainSceneHandler += () =>
        {
            StateMachine(Signal.GotoMainClicked);
        };

        playingState = new PlayingState();
        itemSelectState = itemSelectCanvas;
        pausedState = pauseCanvas;
        gameOverState = gameOverCanvas;
        gameClearState = gameClearCanvas;
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
        GameClear,
        OnPauseClicked,
        OnResumeClicked,
        OnItemSelectDone,
        GotoMainClicked
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

        Debug.Log("Signal : " + signal);
        Debug.Log("Current State : " + currentState);

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
                case Signal.GameOver:
                    gameOverCanvas.gameObject.SetActive(true);
                    SetState(gameOverState);
                    break;
                case Signal.GameClear:
                    gameClearCanvas.gameObject.SetActive(true);
                    SetState(gameClearState);
                    break;
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
                case Signal.GotoMainClicked:
                    StartCoroutine(readyState.FadeOut(() =>
                    {
                        Time.timeScale = 1;
                        SceneManager.LoadScene("MainScene");
                    }));
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
        else if (currentState == gameOverState)
        {
            switch (signal)
            {
                case Signal.GotoMainClicked:
                    StartCoroutine(readyState.FadeOut(() =>
                    {
                        Time.timeScale = 1;
                        SceneManager.LoadScene("MainScene");
                    }));
                    break;
            }
        }
        else if (currentState == gameClearState)
        {
            switch (signal)
            {
                case Signal.GotoMainClicked:
                    StartCoroutine(readyState.FadeOut(() =>
                    {
                        Time.timeScale = 1;
                        SceneManager.LoadScene("MainScene");
                    }));
                    break;
            }

        }
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
            Canvas canvas = new GameObject("FadeCanvas", typeof(Canvas)).GetComponent<Canvas>();
            canvas.transform.SetParent(CanvasManager.instance.transform);
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
            RectTransform rect = canvas.GetComponent<RectTransform>();
            rect.anchorMax = new Vector2(1, 1);
            rect.anchorMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
            rect.offsetMin = new Vector2(0, 0);
            rect.localScale = Vector3.one;

            fadeImage = new GameObject("FadeImage", typeof(Image));
            fadeImage.transform.SetParent(canvas.transform);
            rect = fadeImage.GetComponent<RectTransform>();
            rect.anchorMax = new Vector2(1, 1);
            rect.anchorMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
            rect.offsetMin = new Vector2(0, 0);
            rect.localScale = Vector3.one;

            Image image = fadeImage.GetComponent<Image>();
            image.color = new Color(0, 0, 0, 1);
            canvas.transform.SetAsFirstSibling();
            gameObject.SetActive(true);
        }

        public IEnumerator FadeIn()
        {
            float elpasedTime = 0;
            yield return new WaitForSeconds(1);
            Image image = fadeImage.GetComponent<Image>();
            while (elpasedTime < fadeTime)
            {
                elpasedTime += Time.unscaledDeltaTime;
                image.color = new Color(0, 0, 0, 1 - elpasedTime);
                yield return null;
            }
        }

        public IEnumerator FadeOut(Action callback)
        {
            float elpasedTime = 0;
            Image image = fadeImage.GetComponent<Image>();
            while (elpasedTime < fadeTime)
            {
                elpasedTime += Time.unscaledDeltaTime;
                image.color = new Color(0, 0, 0, elpasedTime);
                yield return null;
            }
            callback();
        }

        public void OnEnter()
        {

        }

        public void OnExit()
        {

        }
    }
}


