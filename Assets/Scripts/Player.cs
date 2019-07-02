using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance = null;
    public Transform PlayerTransform;
    public int MaxHealth;
    public int CurrentHealth;
    public int Damage;
    public int Bullets;
    public int Score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void IncreaseMaxHealth(int amount)
    {
        MaxHealth += amount;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        if(CurrentHealth <= 0)
        {
            GameOver();
        }
    }

    public void RegenerateHealth(int amount)
    {
        if(CurrentHealth < MaxHealth)
        {
            if (CurrentHealth + amount < MaxHealth)
            {
                CurrentHealth += amount;
            }
            else
            {
                CurrentHealth = MaxHealth;
            }
        }
    }

    public void IncreaseScore(int amount)
    {
        Score += amount;
    }

    private void GameOver()
    {
        Debug.Log("You died");
    }
}
