using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SpawnPattern
{
    public Vector2 spawnTimeRange;  // 초 단위
    public int spawnPeriod;  // 초 단위
    public PoolType virusID;
    public int spawnMonsterNum;
    public bool ignoreMaxVirusNum;  // true일 경우 maxVirusNum을 넘더라도 무시하고 스폰 (ex: 보스)
    public bool randomAroundPlayer;
    public bool randomAllOverMap;  // 맵 경계 내부 전체에서 랜덤하게 스폰

    [Header("Random Around Player인 경우만")]
    //public Vector2 offsetFromPlayer;
    public float offsetFromPlayer;
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
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int maxVirusNum = 100;
    [SerializeField] private GameObject turretPoints;
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
        Vector2 randomPoint = GetRandomPointAroundPlayer(spawnRange.x, spawnRange.y, Vector2.zero);

        Spawn(index, randomPoint.x, randomPoint.y);
    }

    // (x, z) 위치에 바이러스 스폰
    public void Spawn(PoolType index, float x, float z, bool withEffect = true)
    {
        Vector3 spawnPosition = new Vector3(x, 0, z);

        // GameObject virus = PoolManager.instance.GetObject
        // (
        //     index,
        //     spawnPosition,
        //     Quaternion.LookRotation(player.transform.position - spawnPosition)
        // );

        // virus.GetComponent<VirusBehaviour>().OnDie += OnVirusDestroyed;

        VirusSpawnFactory.instance.SpawnVirus(index, spawnPosition, withEffect, (VirusBehaviour virus) =>
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
            Vector2 offsetPoint = GetOffsetPoint(spawnPattern.offsetFromPlayer);

            for (int i = 0; i < spawnPattern.spawnMonsterNum; i++)
            {
                if (currentVirusNum >= maxVirusNum && !spawnPattern.ignoreMaxVirusNum)
                {
                    continue;
                }

                Vector2 randomPoint = spawnPattern.randomAllOverMap ? GetRandomPointAllOverMap() :
                    (spawnPattern.randomAroundPlayer ?
                    GetRandomPointAroundPlayer(spawnPattern.spawnRange.x, spawnPattern.spawnRange.y, offsetPoint) :
                    spawnPattern.spawnPoints[i]);
                // float x = randomPoint.x + spawnPattern.offsetFromPlayer.x;
                // float z = randomPoint.y + spawnPattern.offsetFromPlayer.y;

                Spawn(virusPoolType, randomPoint.x, randomPoint.y);
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

    // 맵 경계 내부 전체에서 랜덤한 점 반환
    private Vector2 GetRandomPointAllOverMap()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector2 point = MapBoundary.instance.GetRandomPoint();

            if (IsValidPoint(point))
            {
                return point;
            }
        }

        Debug.LogWarning("Failed to find a valid point in 100 attempts");
        return Vector2.zero;
    }

    // 플레이어로부터 반지름이 minRadius ~ maxRadius인 원 안의 랜덤한 점 반환
    private Vector2 GetRandomPointAroundPlayer(float minRadius, float maxRadius, Vector2 offsetFromPlayer)
    {
        for (int i = 0; i < 100; i++)
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
            float x = radius * Mathf.Cos(angle) + player.transform.position.x;
            float y = radius * Mathf.Sin(angle) + player.transform.position.z;
            Vector2 point = new Vector2(x, y) + offsetFromPlayer;

            if (IsValidPoint(point))
            {
                return point;
            }
        }

        Debug.LogWarning("Failed to find a valid point in 100 attempts");
        return Vector2.zero;
    }

    private Vector2 GetOffsetPoint(float radius)
    {
        if (radius == 0)
        {
            return Vector2.zero;
        }

        for (int i = 0; i < 100; i++)
        {
            // Random angle in radians
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);

            // Convert polar coordinates to Cartesian coordinates
            Vector2 circlePoint = radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            float x = circlePoint.x + player.transform.position.x;
            float y = circlePoint.y + player.transform.position.z;
            Vector2 point = new Vector2(x, y);

            if (IsValidPoint(point))
            {
                return circlePoint;
            }
        }

        Debug.LogWarning("Failed to find a valid point in 100 attempts");
        return Vector2.zero;
    }

    private bool IsValidPoint(Vector2 point)
    {
        if (!MapBoundary.instance.PointInBoundary(point))
        {
            Debug.Log("Point not in boundary");
            return false;
        }

        if (Physics.CheckSphere(new Vector3(point.x, 0, point.y), 0.5f, obstacleLayer)) // TODO? : radius 조정
        {
            return false;
        }

        return true;
    }

    public void OnVirusDestroyed(VirusBehaviour virus)
    {
        // virus.OnDie -= OnVirusDestroyed;
        currentVirusNum--;  // synchronization issue?
    }

    public void SpawnTurret(int idx)
    {
        Transform turretPoint = turretPoints.transform.GetChild(idx);

        Debug.Log("Turret Spawned");
        PoolManager.instance.GetObject
        (
            PoolType.RedZone,
            turretPoint.position,
            turretPoint.rotation
        );
    }
}


#if UNITY_EDITOR
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
#endif
