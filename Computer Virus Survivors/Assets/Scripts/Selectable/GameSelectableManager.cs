using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;

public class GameSelectableManager : MonoBehaviour
{
    [SerializeField] private List<WeaponData> weapons;
    [SerializeField] private List<ItemData> items;

    private void OnEnable()
    {
        if (weapons != null)
        {
            foreach (WeaponData weapon in weapons)
            {
                weapon.Initialize();
            }
        }
        if (items != null)
        {
            foreach (ItemData item in items)
            {
                item.Initialize();
            }
        }
    }


    /// <summary>
    /// 인스펙터 창에서 WeaponData를 자동으로 추가하는 에디터. 버튼 딸깍 자동화
    /// </summary>
    [CustomEditor(typeof(GameSelectableManager))]
    public class WeaponManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Auto Populate Weapon List"))
            {
                GameSelectableManager manager = (GameSelectableManager) target;
                string[] guids = AssetDatabase.FindAssets("t:WeaponData", new[] { "Assets/Scripts/Scriptable/Weapons" });
                manager.weapons = new List<WeaponData>();

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    WeaponData weaponData = AssetDatabase.LoadAssetAtPath<WeaponData>(path);
                    if (weaponData != null)
                    {
                        manager.weapons.Add(weaponData);
                    }
                }
                Debug.Log("Weapon list populated with " + manager.weapons.Count + " weapon(s).");

                guids = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/Scripts/Scriptable/Items" });
                manager.items = new List<ItemData>();

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    ItemData itemData = AssetDatabase.LoadAssetAtPath<ItemData>(path);
                    if (itemData != null)
                    {
                        manager.items.Add(itemData);
                    }
                }
                Debug.Log("Item list populated with " + manager.items.Count + " item(s).");

                EditorUtility.SetDirty(manager);
            }
        }
    }
}


