using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    ExpGem = 0,
    Virus_Weak,
    Virus_Trojan
}

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

    // Get함수는 Instantiate와 동일한 작동을 해야 하는 함수
    public GameObject Get(PoolType index, Vector3 position, Quaternion rotation)
    {
        GameObject selected = null;

        foreach (GameObject obj in pool[(int) index])
        {
            if (!obj.activeSelf)
            {
                selected = obj;
                selected.SetActive(true);
                // 세진 : Get은 당장 사용 가능한 오브젝트를 반환하는 것이 의미상 맞다고 생각합니다.
                // 여기에서 SetActive(true)를 해주나, 아래에서 Instantiate를 해주나 해당 오브젝트의
                // OnEnable()은 호출되기 때문에, PoolManager에서 관리하는 오브젝트들의 초기화는 OnEnable()에서 하면 될 겁니다.
                break;
            }
        }

        if (!selected)
        {
            selected = Instantiate(prefabs[(int) index]);
            pool[(int) index].Add(selected);
        }

        selected.transform.position = position;
        selected.transform.rotation = rotation;

        return selected;
    }
}
