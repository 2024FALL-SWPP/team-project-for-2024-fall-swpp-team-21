using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private List<GameObject>[] pool;  // TODO: 프리팹들의 인덱스를 따로 관리해줘야 할 듯
    // 현재 - 0: ExpGem, 1: Virus_Weak, 2: Virus_Trojan

    // Start is called before the first frame update
    private void Start()
    {
        pool = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < prefabs.Length; i++)
        {
            pool[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index, Vector3 position)
    {
        GameObject selected = null;

        foreach (GameObject obj in pool[index])
        {
            if (!obj.activeSelf)
            {
                selected = obj;
                break;
            }
        }

        if (!selected)
        {
            selected = Instantiate(prefabs[index], transform);
            pool[index].Add(selected);
        }

        selected.transform.position = position;

        return selected;
    }
}
