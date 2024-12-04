using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// SelectableBehaviour에서 이름, 현재 레벨, 최대 레벨, 설명만 저장하는 클래스
/// </summary>
public class SelectionInfo
{
    public string objectName;
    public int currentLevel;
    public int maxLevel;
    public string explanation;
    public bool isWeapon;

    public SelectionInfo(SelectableBehaviour selectableBehaviour)
    {
        this.objectName = selectableBehaviour.ObjectName;
        this.currentLevel = selectableBehaviour.CurrentLevel;
        this.maxLevel = selectableBehaviour.MaxLevel;
        this.explanation = selectableBehaviour.Explanations[currentLevel];
        this.isWeapon = selectableBehaviour is WeaponBehaviour;
    }

    public override string ToString()
    {
        string levelChange = "NEW!";
        if (currentLevel != 0 && currentLevel + 1 < maxLevel)
        {
            levelChange = currentLevel + " -> " + (currentLevel + 1);
        }
        else if (currentLevel + 1 == maxLevel)
        {
            levelChange = "MAX LEVEL!";
        }

        return string.Format("<{0}>\n" +
                            "{1}\n" +
                            "{2}\n" +
                            "\n" +
                            "{3}"
                            , isWeapon ? "무기" : "아이템", objectName, levelChange, explanation);
        /*
        <무기>
        패킷 스트림
        1 -> 2

        공격속도 15% 증가
        */
    }
}

public class SelectableManager : Singleton<SelectableManager>
{
    [SerializeField]
    private List<GameObject> weaponPrefabs;

    [SerializeField]
    private List<GameObject> itemPrefabs;

    private List<SelectableBehaviour> allWeaponBehaviour;
    private List<SelectableBehaviour> allItemBehaviour;

    private List<SelectableBehaviour> playerWeapons;
    private List<SelectableBehaviour> playerItems;

    public override void Initialize()
    {
        allWeaponBehaviour = new List<SelectableBehaviour>();
        allItemBehaviour = new List<SelectableBehaviour>();

        foreach (var obj in weaponPrefabs)
        {
            allWeaponBehaviour.Add(obj.GetComponent<SelectableBehaviour>());
        }

        foreach (var obj in itemPrefabs)
        {
            allItemBehaviour.Add(obj.GetComponent<SelectableBehaviour>());
        }

        Debug.Log("WeaponInfos count : " + allWeaponBehaviour.Count);
        Debug.Log("ItemInfos count : " + allItemBehaviour.Count);

    }

    /// <summary>
    /// 전체 아이템 중 플레이어의 만렙인 아이템을 제외하여 랜덤하게 count개의 아이템을 선택하여 정보를 반환
    /// 현재로선 무기와 아이템 모두 최대 보유 개수가 최대 6으로 설정되어 있음
    /// 하지만 이 메소드를 통하지 않은 아이템 획득은 막지 않음
    /// </summary>
    /// <param name="playerWeapons">플레이어의 보유중인 무기 리스트</param>
    /// <param name="playerItems">플레이어의 보유중인 아이템 리스트</param>
    /// <param name="count">선택지 개수</param>
    /// <returns></returns>
    public List<SelectionInfo> GetChoices(int count = 3)
    {
        UpdatePlayerSeletableInfos();

        List<SelectableBehaviour> candidateWeapons = new List<SelectableBehaviour>();
        List<SelectableBehaviour> candidateItems = new List<SelectableBehaviour>();

        // 플레이어 무기가 6개 미만이면 플레이어가 보유하지 않은 무기를 후보로 추가
        if (playerWeapons.Count < 6)
        {
            candidateWeapons = allWeaponBehaviour.Except(playerWeapons).ToList();
        }
        // 플레이어 무기 중 만렙이 아닌 무기를 후보로 추가
        foreach (var playerWeapon in playerWeapons)
        {
            Debug.Log("PlayerWeapon : " + playerWeapon.CurrentLevel);
            if (!playerWeapon.IsMaxLevel())
            {
                candidateWeapons.Add(playerWeapon);
            }
        }


        // 플레이어 아이템이 6개 미만이면 플레이어가 보유하지 않은 아이템을 후보로 추가
        if (playerItems.Count < 6)
        {
            candidateItems = allItemBehaviour.Except(playerItems).ToList();
        }
        // 플레이어 아이템 중 만렙이 아닌 아이템을 후보로 추가
        foreach (var playerItem in playerItems)
        {
            if (!playerItem.IsMaxLevel())
            {
                candidateItems.Add(playerItem);
            }
        }

        Debug.Log("CandidateWeapons count : " + candidateWeapons.Count);
        Debug.Log("CandidateItems count : " + candidateItems.Count);

        // 후보 무기, 아이템을 합쳐서 랜덤하게 count개 선택
        List<SelectableBehaviour> totalCandidates = candidateWeapons.Concat(candidateItems).ToList();
        List<SelectionInfo> choices = new List<SelectionInfo>();

        if (totalCandidates.Count == 0)
        {
            // TODO : 후보가 없을 때 처리. 뱀서에선 체력회복 아이템이나 골드를 띄웠음
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                if (totalCandidates.Count == 0)
                {
                    break;
                }
                int randomIndex = Random.Range(0, totalCandidates.Count);
                choices.Add(new SelectionInfo(totalCandidates[randomIndex]));
                totalCandidates.RemoveAt(randomIndex);
            }
        }

        return choices;
    }


    /// <summary>
    /// 아이템 이름을 받아 해당 아이템의 SelectableBehaviour를 반환
    /// 만약 플레이어가 보유하지 않은 아이템이라면 Selectable Prefab을 인스턴스화하여 반환
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public SelectableBehaviour GetSelectableBehaviour(string itemName)
    {

        UpdatePlayerSeletableInfos();

        foreach (var playerWeapon in playerWeapons)
        {
            if (playerWeapon.ObjectName == itemName)
            {
                return playerWeapon;
            }
        }

        foreach (var playerItem in playerItems)
        {
            if (playerItem.ObjectName == itemName)
            {
                return playerItem;
            }
        }

        // 아이템이 없을 경우 프리팹을 찾아 인스턴스화
        GameObject prefab = weaponPrefabs.Find(x => x.GetComponent<SelectableBehaviour>().ObjectName == itemName);
        if (prefab == null)
        {
            prefab = itemPrefabs.Find(x => x.GetComponent<SelectableBehaviour>().ObjectName == itemName);
        }
        if (prefab == null)
        {
            Debug.LogError("No Selectable Prefab Found");
            return null;
        }

        Debug.Log("Prefab : " + prefab.name);

        GameObject newSelectable = Instantiate(prefab, GameManager.instance.Player.transform);
        return newSelectable.GetComponent<SelectableBehaviour>();

    }


    /// <summary>
    /// 플레이어의 현재 보유중인 무기, 아이템 정보 업데이트
    /// </summary>
    private void UpdatePlayerSeletableInfos()
    {
        playerWeapons = GameManager.instance.Player.GetComponent<PlayerController>().playerStat.GetPlayerWeaponInfos();
        playerItems = GameManager.instance.Player.GetComponent<PlayerController>().playerStat.GetPlayerItemInfos();
    }


    /// <summary>
    /// 인스펙터 창에서 자동으로 선택 가능한 오브젝트를 로드하는 버튼을 추가
    /// </summary>
    [CustomEditor(typeof(SelectableManager))]
    public class SelectableManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Load Selectable Objects"))
            {
                LoadSelectableObjects();
            }

        }

        private void LoadSelectableObjects()
        {
            SelectableManager manager = (SelectableManager) target;

            manager.weaponPrefabs = new List<GameObject>();
            manager.itemPrefabs = new List<GameObject>();

            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });

            foreach (string guid in prefabGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (prefab != null && prefab.GetComponent<WeaponBehaviour>() != null)
                {
                    manager.weaponPrefabs.Add(prefab);
                }
                else if (prefab != null && prefab.GetComponent<ItemBehaviour>() != null)
                {
                    if (prefab.name == "I_ItemBase")
                    {
                        continue;
                    }
                    manager.itemPrefabs.Add(prefab);
                }
            }

        }
    }



}
