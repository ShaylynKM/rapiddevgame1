using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 15;
    private int currentHealth;
    public GameObject hitPlayerEffectPrefab;

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        
        if (hitPlayerEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitPlayerEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration); // 根据粒子系统的持续时间来销毁
        }


        if (currentHealth <= 0)
        {
            Die();
        }

        // Log player's health change
        Debug.Log("Player Health Decreased by: " + damage);
    }

    private void Die()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Player Died");
            GameOver(); 
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        Application.Quit();
    }

    // Optional: Method to heal the player
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Log player's health change
        Debug.Log("Player Health Increased by: " + healAmount);
    }
}
