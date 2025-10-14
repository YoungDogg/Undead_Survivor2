using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime;

    [Header("# Player Info")]
    public int health;
    public int maxHealth;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp; 

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;

    private void Awake()
    {
        instance = this;
        maxGameTime = 2 * 10f;
        maxHealth = 100;
        nextExp = new int[] { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    }

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if(gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if(exp == nextExp[level])
        {
            level++;
            exp = 0;

        }
    }
}
