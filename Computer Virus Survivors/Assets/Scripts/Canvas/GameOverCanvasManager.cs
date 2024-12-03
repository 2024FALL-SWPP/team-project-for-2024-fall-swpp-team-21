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
    [SerializeField] private GameObject weaponStatisticsPanel;
    [SerializeField] private GameObject playerInfoPanel;
    // private TextMeshProUGUI youdiedText;
    // private Image gameoverImage;
    // private Color textColor;

    private Animator animator;
    private GameObject weaponSingleStat;

    public override void Initialize()
    {
        animator = GetComponent<Animator>();
        weaponSingleStat = weaponStatisticsPanel.transform.GetChild(0).gameObject;
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

    // private void ShowGotoHomeBtn()
    // {
    //     gotoHomeBtn.SetActive(true);
    // }

    private void InitializeStatistics()
    {
        List<WeaponStatistic> weaponDatas = GameManager.instance.GetWeaponStatistics();
        int totalKillCount = 0;
        int totalDamage = 0;
        for (int i = 0; i < weaponDatas.Count; i++)
        {
            GameObject weaponStat = Instantiate(weaponSingleStat, weaponStatisticsPanel.transform);
            TextMeshProUGUI weaponName = weaponStat.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI weaponKillCount = weaponStat.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI weaponDamage = weaponStat.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            weaponName.text = weaponDatas[i].weaponName;
            weaponKillCount.text = weaponDatas[i].killCount.ToString();
            weaponDamage.text = weaponDatas[i].totalDamage.ToString();

            totalKillCount += weaponDatas[i].killCount;
            totalDamage += weaponDatas[i].totalDamage;
        }

        TextMeshProUGUI valueField;
        // 백신 이름
        valueField = playerInfoPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        valueField.text = GameManager.instance.Player.GetComponent<PlayerController>().playerStatData.characterName;

        // 레벨
        valueField = playerInfoPanel.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        valueField.text = GameManager.instance.Player.GetComponent<PlayerController>().playerStat.PlayerLevel.ToString();

        // 생존 시간
        valueField = playerInfoPanel.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        valueField.text = (int) (GameManager.instance.gameTime / 60) + "m " + (int) (GameManager.instance.gameTime % 60) + "s";

        // empty

        // 총 데미지
        valueField = playerInfoPanel.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();
        valueField.text = totalDamage.ToString();

        // 총 킬 수
        valueField = playerInfoPanel.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();
        valueField.text = totalKillCount.ToString();
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
        InitializeStatistics();
    }

    public void OnExit()
    {
        gotoHomeBtn.SetActive(false);
        gameObject.SetActive(false);
    }


}

