using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    // List 배열 대신 Queue 배열을 쓴다
    // 이 Queue는 '비활성화된' 오브젝트만 담는다.
    Queue<GameObject>[] pools;

    private void Awake()
    {
        // 리스트가 아니라 큐로 초기화한다.
        pools = new Queue<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new Queue<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 1. Queue에 대기 중인 객체이 있는지 확인 (Count >0)
        // 이게 List의 foreach보다 효율적이다. (O(1) 연산)
        if (pools[index].Count > 0)
        {
            // 2. 대기 줄 맨 앞에 객체를 꺼낸다 (Dequeue)
            select = pools[index].Dequeue();
            select.SetActive(true); // 깨우고 보내기
        }
        else
        {
            // 3. 대기줄이 비었으면 (if !select와 동일한 로직)
            // 새 객체를 생성한다.
            select = Instantiate(prefabs[index], transform);
            // 새로 만든 객체는 Queue에 넣지 않고 바로 사용한다.
            // Queue는 대기 중인 객체만 있는다.
        }
        return select;
    }
    // 4. 오브젝트 반납 메서드
    public void Return(int index, GameObject target)
    {
        // 오브젝트를 비활성화하고
        target.SetActive(false);
        // 다시 대기줄 맨 뒤에 넣는다.
        pools[index].Enqueue(target);
    }
}
