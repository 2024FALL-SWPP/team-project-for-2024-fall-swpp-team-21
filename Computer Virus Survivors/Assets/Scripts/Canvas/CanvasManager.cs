using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour, IPlayerStatObserver
{

    public static CanvasManager instance;
    [SerializeField] private PlayerStatEventCaller playerStatEventCaller;

    [SerializeField] private ItemSelectCanvasManager itemSelectCanvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnEnable()
    {
        playerStatEventCaller.StatChanged += OnStatChanged;
    }

    public void OnSelectionDone()
    {
        Time.timeScale = 1;
        itemSelectCanvas.gameObject.SetActive(false);
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
