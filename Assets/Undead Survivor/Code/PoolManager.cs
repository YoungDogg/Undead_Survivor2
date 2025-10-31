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

    // Instantiate할 원본을 저장할 딕셔너리. 프리팹이 많아지면 이 방법이 유리하다.
    private Dictionary<PoolType, GameObject> prefabDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();
        prefabDictionary = new Dictionary<PoolType, GameObject>();

        // 인스펙터에 설정된 poolSettings 목록을 순회한다
        foreach (PoolSetting setting in poolSettings)
        {
            //딕셔너리에 타입을 키로 등록한다.
            // Enemy 타입의 큐
            poolDictionary.Add(setting.type, new Queue<GameObject>());
            //Enemy 타입의 원본 프리팹
            prefabDictionary.Add(setting.type, setting.prefab);
        }
    }

    public GameObject Get(PoolType type)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogError("PoolManager: " + type + "타입의 풀이 없습니다.");
            return null;
        }

        GameObject select = null;
        if (poolDictionary[type].Count > 0)
        {
            select = poolDictionary[type].Dequeue();
        }
        else
        {
            // 큐가 비었으면, Enemy 타입의 원본프리팹을 찾아서 새로 생성
            GameObject prefab = prefabDictionary[type];
            select = Instantiate(prefab, transform);
        }

        select.SetActive(true);
        return select;
    }
    // Return 메서드도 PoolType을 받는다.
    public void Return(PoolType type, GameObject target)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogError("PoolManager: " + type + "타입의 풀이 없습니다.");
            return;
        }

        target.SetActive(false);
        poolDictionary[type].Enqueue(target);
    }
}
