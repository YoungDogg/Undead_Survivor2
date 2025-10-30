using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float durationPerLevel;
    
    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        int numOfDifficultySteps = spawnData.Length;

        if(numOfDifficultySteps > 0)
        {
            durationPerLevel = GameManager.instance.maxGameTime / numOfDifficultySteps;
        }
        else    // avoid division by zero if spawndata is empty
        {
            Debug.LogError("SpawnData array is empty! cannot calculate durationPerLevel.");
            durationPerLevel = GameManager.instance.maxGameTime;
        }

            
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / durationPerLevel), spawnData.Length-1);

        if(timer > spawnData[level].spawnInterval)
        {
            timer = 0f;
            Spawn();
        }   
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(PoolType.Enemy);    // which is enemy
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].transform.position; // 0 is the player
        enemy.GetComponent<Enemy>().Init(spawnData[level]);       
    }
}

[System.Serializable]
public class SpawnData
{    
    public float spawnInterval;
    public int spriteType;
    public int health;
    public float speed;
}