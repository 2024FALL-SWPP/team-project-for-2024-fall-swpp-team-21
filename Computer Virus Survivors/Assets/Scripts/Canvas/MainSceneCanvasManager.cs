using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Rendering.HybridV2;
using UnityEngine.UI;

public abstract class CanvasBase : Singleton<CanvasBase>, IState
{
    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public override void Initialize() { }
}


public class MainSceneCanvasManager : Singleton<MainSceneCanvasManager>
{

    [SerializeField] private TitleCanvasManager titleSceneCanvas;
    [SerializeField] private CharacterSelectCanvas characterSelectCanvas;
    [SerializeField] private StageSelectCanvasManager stageSelectCanvas;

    private CanvasBase playingState;
    private CanvasBase currentState;

    private GameObject selectedPlayer;
    private string selectedStageName;


    private void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        titleSceneCanvas.Initialize();
        characterSelectCanvas.Initialize();
        stageSelectCanvas.Initialize();
        titleSceneCanvas.BtnCliked += (index) =>
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
        characterSelectCanvas.CharacterSelectedHandler += (player) =>
        {
            selectedPlayer = player;
            StateMachine(Signal.OnCharacterSelectDone);
        };
        stageSelectCanvas.GotoPreviousHandler += () =>
        {
            StateMachine(Signal.OnGotoPreviousClicked);
        };
        stageSelectCanvas.StageSelectedHandler += (stageName) =>
        {
            selectedStageName = stageName;
            StateMachine(Signal.OnStageSelectDone);
        };

        playingState = new GameObject("PlayingState").AddComponent<PlayingState>();
        playingState.Initialize();

        currentState = titleSceneCanvas;

        StartCoroutine(((PlayingState) playingState).FadeIn());
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
        void SetState(CanvasBase state)
        {
            currentState.OnExit();
            currentState.gameObject?.SetActive(false);

            currentState = state;
            Debug.Log("State Change : " + state);

            currentState.gameObject?.SetActive(true);
            currentState.OnEnter();
        }

        if (currentState == titleSceneCanvas)
        {
            switch (signal)
            {
                case Signal.OnGameStartClicked:
                    SetState(characterSelectCanvas);
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
        else if (currentState == characterSelectCanvas)
        {
            switch (signal)
            {
                case Signal.OnCharacterSelectDone:
                    SetState(stageSelectCanvas);
                    break;
                case Signal.OnGotoPreviousClicked:
                    SetState(titleSceneCanvas);
                    break;
            }
        }
        else if (currentState == stageSelectCanvas)
        {
            switch (signal)
            {
                case Signal.OnStageSelectDone:
                    SetState(playingState);
                    break;
                case Signal.OnGotoPreviousClicked:
                    SetState(characterSelectCanvas);
                    break;
            }
        }
        else if (currentState == playingState)
        {
            switch (signal)
            {
                case Signal.OnGameCleared:
                    SetState(titleSceneCanvas);
                    break;
            }
        }
    }

    public string GetSelectedStageName()
    {
        return selectedStageName;
    }

    private class PlayingState : CanvasBase
    {

        private GameObject fadeImage;
        private float fadeTime = 2f;

        public override void Initialize()
        {
            // fadeImage = new GameObject("FadeImage", typeof(Image));
            // fadeImage.transform.SetParent(MainSceneCanvasManager.instance.transform);
            // RectTransform rect = fadeImage.GetComponent<RectTransform>();
            // rect.anchorMax = new Vector2(1, 1);
            // rect.anchorMin = new Vector2(0, 0);
            // rect.offsetMax = new Vector2(0, 0);
            // rect.offsetMin = new Vector2(0, 0);
            // rect.localScale = Vector3.one;
            // Image image = fadeImage.GetComponent<Image>();
            // image.color = new Color(0, 0, 0, 0);
            // fadeImage.transform.SetAsFirstSibling();
            // gameObject.SetActive(false);
            Canvas canvas = new GameObject("FadeCanvas", typeof(Canvas)).GetComponent<Canvas>();
            canvas.transform.SetParent(MainSceneCanvasManager.instance.transform);
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

        public override void OnEnter()
        {
            // 씬 전환
            StartCoroutine(StageLoad());
        }

        private IEnumerator StageLoad()
        {
            float elpasedTime = 0;
            Image image = fadeImage.GetComponent<Image>();
            while (elpasedTime < fadeTime)
            {
                elpasedTime += Time.deltaTime;
                image.color = new Color(0, 0, 0, elpasedTime / fadeTime);
                yield return null;
            }
            SceneManager.LoadScene(MainSceneCanvasManager.instance.GetSelectedStageName());
        }

        public IEnumerator FadeIn()
        {
            float elpasedTime = 0;
            Image image = fadeImage.GetComponent<Image>();
            yield return new WaitForSeconds(2.0f);
            while (elpasedTime < fadeTime)
            {
                elpasedTime += Time.deltaTime;
                image.color = new Color(0, 0, 0, 1 - elpasedTime / fadeTime);
                yield return null;
            }
        }

        public override void OnExit()
        {
            ;
        }
    }
}


