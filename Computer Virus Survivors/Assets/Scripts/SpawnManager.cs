using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class SpawnPattern
{
    public Vector2 spawnTimeRange;  // 초 단위
    public int spawnPeriod;  // 초 단위
    public PoolType virusID;
    public int spawnMonsterNum;
    public bool ignoreMaxVirusNum;  // true일 경우 maxVirusNum을 넘더라도 무시하고 스폰 (ex: 보스)
    public bool isSpawnPointRandom;

    [Header("랜덤인 경우만")]
    public Vector2 offsetFromPlayer;
    public Vector2 spawnRange;

    [Header("랜덤이 아닌 경우만 (Spawn Monster Num만큼 필요)")]
    public List<Vector2> spawnPoints;
}

public class SpawnPatternList
{
    public List<SpawnPattern> patterns;

    public static string CreateJSON(SpawnPatternList spawnPatternList)
    {
        return JsonUtility.ToJson(spawnPatternList);
    }
}



public class SpawnManager : Singleton<SpawnManager>
{

    [SerializeField] private Vector2 spawnRange;  // for test: 플레이어 주위 스폰 범위
    [SerializeField] private int maxVirusNum = 100;
    public List<SpawnPattern> spawnPatterns;

    private GameObject player;
    private int currentVirusNum = 0;
    // private List<Coroutine> runningCoroutines = new List<Coroutine>();

    public override void Initialize()
    {
        player = GameManager.instance.Player;
        //LoadSpawnPattern();
        spawnPatterns.Sort((a, b) => a.spawnTimeRange.x.CompareTo(b.spawnTimeRange.x));
    }

    public void StartSpawnManager()
    {
        StartCoroutine(MainSpawnCoroutine());
    }

    // for test: 플레이어 주위 스폰
    public void Spawn(PoolType index)
    {
        Vector2 randomPoint = GetRandomPoint(spawnRange.x, spawnRange.y);

        Spawn(index, randomPoint.x, randomPoint.y);
    }

    // (x, z) 위치에 바이러스 스폰
    public void Spawn(PoolType index, float x, float z)
    {
        Vector3 spawnPosition = player.transform.position + new Vector3(x, 0, z);

        // GameObject virus = PoolManager.instance.GetObject
        // (
        //     index,
        //     spawnPosition,
        //     Quaternion.LookRotation(player.transform.position - spawnPosition)
        // );

        // virus.GetComponent<VirusBehaviour>().OnDie += OnVirusDestroyed;

        VirusSpawnFactory.instance.SpawnVirus(index, spawnPosition, (VirusBehaviour virus) =>
        {
            virus.OnDie += OnVirusDestroyed;
            currentVirusNum++; // synchronization issue?
        });
    }


    // 메인 코루틴: spawn pattern 코루틴들을 순서대로 시작 시간에 맞춰 실행
    private IEnumerator MainSpawnCoroutine()
    {
        foreach (SpawnPattern spawnPattern in spawnPatterns)
        {
            yield return new WaitUntil(() => GameManager.instance.gameTime >= spawnPattern.spawnTimeRange.x);
            StartCoroutine(SpawnPatternCoroutine(spawnPattern));
            //runningCoroutines.Add(StartCoroutine(SpawnPatternCoroutine(spawnPattern)));
        }
    }

    // spawn pattern 코루틴: spawn pattern에 따라 바이러스 스폰
    private IEnumerator SpawnPatternCoroutine(SpawnPattern spawnPattern)
    {
        PoolType virusPoolType = spawnPattern.virusID;
        while (GameManager.instance.gameTime <= spawnPattern.spawnTimeRange.y)
        {
            for (int i = 0; i < spawnPattern.spawnMonsterNum; i++)
            {
                if (currentVirusNum >= maxVirusNum && !spawnPattern.ignoreMaxVirusNum)
                {
                    continue;
                }

                Vector2 randomPoint = spawnPattern.isSpawnPointRandom ?
                    GetRandomPoint(spawnPattern.spawnRange.x, spawnPattern.spawnRange.y) :
                    spawnPattern.spawnPoints[i];
                float x = randomPoint.x + spawnPattern.offsetFromPlayer.x;
                float z = randomPoint.y + spawnPattern.offsetFromPlayer.y;

                Spawn(virusPoolType, x, z);
            }
            yield return new WaitForSeconds(spawnPattern.spawnPeriod);
        }
    }


    // json 파일에서 spawn pattern list 로드
    public void LoadSpawnPattern()
    {
        string jsonText = File.ReadAllText("spawnPatterns.json");  // path?
        SpawnPatternList spawnPatternList = JsonUtility.FromJson<SpawnPatternList>(jsonText);
        spawnPatterns = spawnPatternList.patterns;
    }

    // 반지름이 minRadius ~ maxRadius인 원 안의 랜덤한 점 반환
    private Vector2 GetRandomPoint(float minRadius, float maxRadius)
    {
        // Ensure minRadius is not greater than maxRadius
        if (minRadius > maxRadius)
        {
            (maxRadius, minRadius) = (minRadius, maxRadius);
        }

        // Random angle in radians
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);

        // Random radius between minRadius and maxRadius
        float radius = UnityEngine.Random.Range(minRadius, maxRadius);

        // Convert polar coordinates to Cartesian coordinates
        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }

    public void OnVirusDestroyed(VirusBehaviour virus)
    {
        virus.OnDie -= OnVirusDestroyed;
        currentVirusNum--;  // synchronization issue?
    }

    // public void SpawnTurret()
    // {
    //     Vector3 spawnPosition = new Vector3(0, -4f, 0);

    //     Debug.Log("Turret Spawned");
    //     PoolManager.instance.GetObject
    //     (
    //         PoolType.Turret,
    //         spawnPosition,
    //         Quaternion.identity
    //     );
    // }
}



/// <summary>
/// JSON 파일로 저장 및 로드하는 에디터
/// </summary>
[CustomEditor(typeof(SpawnManager))]
public class SpawnManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Save Spawn Patterns as JSON"))
        {
            SaveAsJson();
        }

        if (GUILayout.Button("Load Spawn Patterns from JSON"))
        {
            LoadFromJson();
        }
    }

    private void SaveAsJson()
    {
        SpawnManager manager = (SpawnManager) target;

        string jsonString = SpawnPatternList.CreateJSON(new SpawnPatternList { patterns = manager.spawnPatterns });
        try
        {
            File.WriteAllText("spawnPatterns.json", jsonString);
            Debug.Log("Successfully saved Spawn Patterns as JSON");
        }
        catch (Exception e)
        {
            Debug.Log("Failed to save Spawn Patterns as JSON: " + e.Message);
        }
    }

    private void LoadFromJson()
    {
        SpawnManager manager = (SpawnManager) target;

        try
        {
            manager.LoadSpawnPattern();
            Debug.Log("Successfully loaded Spawn Patterns from JSON");
        }
        catch (Exception e)
        {
            Debug.Log("Failed to load Spawn Patterns from JSON: " + e.Message);
        }
    }
}
