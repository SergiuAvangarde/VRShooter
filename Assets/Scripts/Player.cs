using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance = null;
    public GameManager ManagerComponent;
    public Transform PlayerTransform;
    public int MaxHealth;
    public int CurrentHealth;
    public int Damage;
    public int Bullets;
    public int Score;

    [SerializeField]
    private GameObject pistolGun = null;
    [SerializeField]
    private GameObject assaultRiffleGun = null;


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
        CurrentHealth = MaxHealth;
    }

    public void SetAssaultRiffle()
    {
        pistolGun.SetActive(false);
        assaultRiffleGun.SetActive(true);
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
        ManagerComponent.SetScore(Score);
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
