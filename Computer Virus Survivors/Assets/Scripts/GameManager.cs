using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    public event Action GameOverHandler;

    [SerializeField] private GameObject playerPrefab;

    private GameObject player;

    public float gameTime = 0;
    public GameObject Player
    {
        get { return player; }
    }

    private void Awake()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Application.targetFrameRate = -1;
        Initialize();
        CanvasManager.instance.GameStart();
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
        CanvasManager.instance.Initialize();
        Debug.Log("CanvasManager Initialized");
        SpawnManager.instance.Initialize();
        Debug.Log("SpawnManager Initialized");
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
        gameTime = 0;
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        GameOverHandler?.Invoke();
    }

    public void GameClear()
    {
        Debug.Log("GameClear");
        StartCoroutine(TimeScaleSlowDown());
        // TODO: GameClear UI
    }

    private IEnumerator TimeScaleSlowDown()
    {
        // reduce time scale to 0
        while (Time.timeScale > 0)
        {
            Time.timeScale = Mathf.Max(0, Time.timeScale - 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
