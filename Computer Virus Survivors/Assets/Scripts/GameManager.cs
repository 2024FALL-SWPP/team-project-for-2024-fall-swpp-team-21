using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject player;

    public float gameTime = 0;
    public GameObject Player
    {
        get { return player; }
    }

    private void Awake()
    {
        Initialize();
        GameStart();
    }

    // Update is called once per frame
    private void Update()
    {
        gameTime += Time.deltaTime;
    }

    public override void Initialize()
    {
        PoolManager.instance.Initialize();
        Debug.Log("PoolManager Initialized");
        SpawnManager.instance.Initialize();
        Debug.Log("SpawnManager Initialized");
        CanvasManager.instance.Initialize();
        Debug.Log("CanvasManager Initialized");
        SelectableManager.instance.Initialize();
        Debug.Log("SelectableManager Initialized");
        CameraController.instance.Initialize();
        Debug.Log("CameraController Initialized");
        Debug.Log("GameManager Initialized");
    }

    private void GameStart()
    {
        Debug.Log("GameStart");
        SpawnManager.instance.StartSpawnManager();
        player.GetComponent<PlayerController>().Initialize();
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        Time.timeScale = 0;
        // TODO
    }

    public void GameClear()
    {
        Debug.Log("GameClear");
        Time.timeScale = 0;
        // TODO
    }
}
