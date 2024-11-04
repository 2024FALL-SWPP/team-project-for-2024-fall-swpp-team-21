using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameSelectableManager : MonoBehaviour
{
    [SerializeField] private List<WeaponData> weapons;
    //[SerializeField] private ItemData[] items;

    private void OnEnable()
    {
        if (weapons != null)
        {
            foreach (WeaponData weapon in weapons)
            {
                weapon.Initialize();
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

                EditorUtility.SetDirty(manager);
                Debug.Log("Weapon list populated with " + manager.weapons.Count + " weapon(s).");
            }
        }
    }
}
