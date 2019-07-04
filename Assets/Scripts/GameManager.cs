using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player PlayerComponent;
    public SpawnEnemies EnemySpawner;
    public int Score = 0;

    private void Update()
    {
        if (Score > 100 && Score < 500)
        {
            EnemySpawner.spawnTimer = 8;
        }
        else if (Score > 500 && Score < 1000)
        {
            EnemySpawner.spawnTimer = 6;
        }
        else if (Score > 1000 && Score < 2000)
        {
            EnemySpawner.spawnTimer = 4;
            PlayerComponent.SetAssaultRiffle();
        }
        else if (Score > 2000 && Score < 5000)
        {
            EnemySpawner.spawnTimer = 3;
        }
        else if (Score > 5000 && Score < 10000)
        {
            EnemySpawner.spawnTimer = 2;
        }
        else if (Score > 10000)
        {
            EnemySpawner.spawnTimer = 1;
        }
    }

    public void SetScore(int totalScore)
    {
        Score = totalScore;
    }
}
