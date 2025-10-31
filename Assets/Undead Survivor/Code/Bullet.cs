using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float damage;
    public int pierceCount;

    // 이 총알이 몇 번 풀에 속하는지 알아야 함.
    // 0번이 총알 프리팹이라고 가정.
    public int poolIndex = 2; // <-- PoolManager 프리팹이 현재 0: 에너미, 1: 근접무기, 2: 원거리 무기

    Rigidbody2D rigid;
    PoolManager poolManager;    // 풀 매니저 참조

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        poolManager = GameManager.instance.pool;
    }

    public void Init(float damage, int pierceCount, Vector3 dir)
    {
        this.damage = damage;
        this.pierceCount = pierceCount;

        if(pierceCount >= 0)
        {
            rigid.velocity = dir * bulletSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || pierceCount == -100)
        {
            return;
        }
        pierceCount--;

        if(pierceCount < 0)
        {
            rigid.velocity = Vector2.zero;
            //gameObject.SetActive(false);
            poolManager.Return(PoolType.Bullet, gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || pierceCount == -100)
            return;

        gameObject.SetActive(false);
        poolManager.Return(PoolType.Bullet, gameObject);
    }
}
