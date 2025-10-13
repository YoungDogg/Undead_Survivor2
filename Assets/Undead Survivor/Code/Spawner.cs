using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > 0.2f)
        {
            timer = 0f;
            Spawn();
        }   
    }

    void Spawn()
    {
        int num_of_prefabs = GameManager.instance.pool.prefabs.Length;
        GameObject enemy = GameManager.instance.pool.Get(Random.Range(0,num_of_prefabs));
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].transform.position; // 0 is the player
        
    }
}
