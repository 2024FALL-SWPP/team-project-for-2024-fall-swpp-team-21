using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvasManager : Singleton<GameOverCanvasManager>, IState
{

    public event Action GotoHomeBtnHandler;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gotoHomeBtn;

    // private TextMeshProUGUI youdiedText;
    // private Image gameoverImage;
    // private Color textColor;

    private Animator animator;

    public override void Initialize()
    {
        animator = GetComponent<Animator>();
        // gameoverImage = gameOverPanel.GetComponent<Image>();
        // youdiedText = gameOverPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        // textColor = youdiedText.color;
        gotoHomeBtn.SetActive(false);
        gameObject.SetActive(false);
    }

    // private IEnumerator ShowYouDied()
    // {
    //     float timeElapsed = 0f;
    //     while (timeElapsed < 1f)
    //     {
    //         timeElapsed += Time.unscaledDeltaTime;

    //         gameoverImage.color = new Color(0, 0, 0, timeElapsed);
    //         youdiedText.color = new Color(textColor.r, textColor.g, textColor.b, timeElapsed);
    //         yield return null;
    //     }

    //     yield return new WaitForSecondsRealtime(1f);

    //     ShowGotoHomeBtn();
    // }

    private void ShowGotoHomeBtn()
    {
        gotoHomeBtn.SetActive(true);
    }

    public void GotoHomeBtnClicked()
    {
        GotoHomeBtnHandler?.Invoke();
    }

    public void OnEnter()
    {
        transform.SetAsLastSibling();
        // StartCoroutine(ShowYouDied());
        animator.SetBool("b_GameOver", true);
    }

    public void OnExit()
    {
        gotoHomeBtn.SetActive(false);
        gameObject.SetActive(false);
    }


}

