using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    public event Action GameOverHandler;
    public event Action GameClearHandler;
    public event Action GotoMainSceneHandler;
    [SerializeField] private GameObject playerPrefab;

    private GameObject player;
    private WeaponStatistics weaponStatistics;

    public float gameTime = 0;
    public GameObject Player
    {
        get { return player; }
    }

    private void Awake()
    {

        weaponStatistics = new WeaponStatistics();

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
        player.GetComponent<PlayerController>().statEventCaller.ClearSubscribers();
        StartCoroutine(TimeScaleSlowDown(() =>
        {
            GameOverHandler?.Invoke();
        }));
    }

    public void GameClear()
    {
        Debug.Log("GameClear");
        player.GetComponent<PlayerController>().statEventCaller.ClearSubscribers();
        StartCoroutine(TimeScaleSlowDown(() =>
        {
            GameClearHandler?.Invoke();
        }));
    }

    public void GotoMainScene()
    {
        player.GetComponent<PlayerController>().statEventCaller.ClearSubscribers();
        GotoMainSceneHandler?.Invoke();
    }

    private IEnumerator TimeScaleSlowDown(Action callback)
    {
        // reduce time scale to 0
        while (Time.timeScale > 0)
        {
            Time.timeScale = Mathf.Max(0, Time.timeScale - 0.1f);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return new WaitForSecondsRealtime(2.0f);
        callback();
    }

    public void AddWeaponData(FinalWeaponData weaponData)
    {
        weaponStatistics.AddWeaponData(weaponData);
    }

    public List<WeaponStatistic> GetWeaponStatistics()
    {
        return weaponStatistics.GetWeaponStatistics();
    }

    public class WeaponStatistics
    {
        private List<FinalWeaponData> weaponDatas;

        public WeaponStatistics()
        {
            weaponDatas = new List<FinalWeaponData>();
        }

        public void AddWeaponData(FinalWeaponData weaponData)
        {
            weaponDatas.Add(weaponData);
        }

        public List<WeaponStatistic> GetWeaponStatistics()
        {
            List<WeaponStatistic> weaponStatistics = new List<WeaponStatistic>();
            foreach (FinalWeaponData weaponData in weaponDatas)
            {
                weaponStatistics.Add(new WeaponStatistic(weaponData.weaponName, weaponData.stat_totalDamage, weaponData.stat_killcount));
            }
            return weaponStatistics;
        }
    }
}

public class WeaponStatistic
{
    public string weaponName;
    public int totalDamage;
    public int killCount;

    public WeaponStatistic(string weaponName, int totalDamage, int killCount)
    {
        this.weaponName = weaponName;
        this.totalDamage = totalDamage;
        this.killCount = killCount;
    }
}
