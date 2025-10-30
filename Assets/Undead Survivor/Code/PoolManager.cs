using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Enemy, Shovel, Bullet
}

[System.Serializable]
public class PoolSetting
{
    public PoolType type;
    public GameObject prefab;
}

public class PoolManager : MonoBehaviour
{
    public List<PoolSetting> poolSettings;

    //public GameObject[] prefabs;

    //private Queue<GameObject>[] pools;
    // PoolType을 키로 사용해서 Queue를 찾는다.
    private Dictionary<PoolType, Queue<GameObject>> poolDictionary;

    // Instantiate할 원본을 저장할 딕셔너리
    private Dictionary<PoolType, GameObject> prefabDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();
        prefabDictionary = new Dictionary<PoolType, GameObject>();

        pools = new Queue<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new Queue<GameObject>();
        }
    }

    public GameObject Get(PoolType type)
    {
        // Enum을 int로 형변환해서 인덱스로 쓴다
        int index = (int)type;

        GameObject select = null;
        if (pools[index].Count > 0)
        {
            select = pools[index].Dequeue();
        }
        else
        {
            select = Instantiate(prefabs[index], transform);
        }

        select.SetActive(true);
        return select;
    }
    public void Return(PoolType type, GameObject target)
    {
        // Enum을 int로 형변환하기
        int index = (int)type;
        target.SetActive(false);
        pools[index].Enqueue(target);
    }
}
