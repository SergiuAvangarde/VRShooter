using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameManager ManagerComponent;
    public Transform PlayerTransform;
    public Text ScoreText;
    public Text PlayerHealth;
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
        CurrentHealth = MaxHealth;
        PlayerHealth.text = $"HP: {CurrentHealth}/{MaxHealth}";
        ScoreText.text = $"Score: {Score}";
    }

    public void SetAssaultRiffle()
    {
        pistolGun.SetActive(false);
        assaultRiffleGun.SetActive(true);
    }

    public void IncreaseMaxHealth(int amount)
    {
        MaxHealth += amount;
        CurrentHealth = MaxHealth;
        PlayerHealth.text = $"HP: {CurrentHealth}/{MaxHealth}";
    }

    public void TakeDamage(int amount)
    {
        PlayerHealth.text = $"HP: {CurrentHealth}/{MaxHealth}";
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
        ScoreText.text = $"Score: {Score}";
        ManagerComponent.SetScore(Score);
    }

    private void GameOver()
    {
        Score = 0;
        ManagerComponent.SetScore(Score);
        CurrentHealth = MaxHealth;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
