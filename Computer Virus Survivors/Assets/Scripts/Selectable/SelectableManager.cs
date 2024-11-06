using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// SelectableBehaviour에서 이름, 현재 레벨, 최대 레벨, 설명만 저장하는 클래스
/// </summary>
public class SelectionInfo
{
    public SelectableBehaviour selectableBehaviour;
    public string itemName;
    public int currentLevel;
    public int maxLevel;
    public string explanation;

    public SelectionInfo(SelectableBehaviour selectableBehaviour)
    {
        this.selectableBehaviour = selectableBehaviour;
        this.itemName = selectableBehaviour.ObjectName;
        this.currentLevel = selectableBehaviour.CurrentLevel;
        this.maxLevel = selectableBehaviour.MaxLevel;
        this.explanation = selectableBehaviour.Explanations[currentLevel];
    }

    public override string ToString()
    {
        return string.Format("Item Name: {0}, Current Level: {1}, Max Level: {2}, Explanation: {3}", itemName, currentLevel, maxLevel, explanation);
    }
}

public class SelectableManager : MonoBehaviour
{
    public static SelectableManager instance;

    [SerializeField]
    private List<GameObject> weaponPrefabs = new List<GameObject>();

    [SerializeField]
    private List<GameObject> itemPrefabs = new List<GameObject>();

    private List<SelectableBehaviour> weaponInfos = new List<SelectableBehaviour>();
    private List<SelectableBehaviour> itemInfos = new List<SelectableBehaviour>();


    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var obj in weaponPrefabs)
        {
            weaponInfos.Add(obj.GetComponent<SelectableBehaviour>());
        }

        foreach (var obj in itemPrefabs)
        {
            itemInfos.Add(obj.GetComponent<SelectableBehaviour>());
        }

        Debug.Log("Initialized SelectableManager");
        Debug.Log("WeaponInfos count : " + weaponInfos.Count);
        Debug.Log("ItemInfos count : " + itemInfos.Count);
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
    public List<SelectionInfo> GetChoices(List<SelectableBehaviour> playerWeapons, List<SelectableBehaviour> playerItems, int count = 3)
    {
        List<SelectableBehaviour> candidateWeapons = new List<SelectableBehaviour>();
        List<SelectableBehaviour> candidateItems = new List<SelectableBehaviour>();

        if (playerWeapons.Count < 6)
        {
            candidateWeapons = weaponInfos.Except(playerWeapons).ToList();
        }
        foreach (var playerWeapon in playerWeapons)
        {
            Debug.Log("PlayerWeapon : " + playerWeapon.CurrentLevel);
            if (!playerWeapon.IsMaxLevel())
            {
                candidateWeapons.Add(playerWeapon);
            }
        }

        if (playerItems.Count < 6)
        {
            candidateItems = itemInfos.Except(playerItems).ToList();
        }
        foreach (var playerItem in playerItems)
        {
            if (!playerItem.IsMaxLevel())
            {
                candidateItems.Add(playerItem);
            }
        }

        Debug.Log("CandidateWeapons count : " + candidateWeapons.Count);
        Debug.Log("CandidateItems count : " + candidateItems.Count);

        List<SelectableBehaviour> totalCandidates = candidateWeapons.Concat(candidateItems).ToList();
        List<SelectionInfo> choices = new List<SelectionInfo>();

        if (totalCandidates.Count == 0)
        {
            // TODO : 후보가 없을 때 처리. 뱀서에선 체력회복 아이템이나 골드를 띄웠음
        }
        else if (totalCandidates.Count < count)
        {
            foreach (var candidate in totalCandidates)
            {
                choices.Add(new SelectionInfo(candidate));
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, totalCandidates.Count);
                choices.Add(new SelectionInfo(totalCandidates[randomIndex]));
                totalCandidates.RemoveAt(randomIndex);
            }
        }

        return choices;
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
                    manager.itemPrefabs.Add(prefab);
                }
            }

        }
    }



}
