using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 15;
    private int currentHealth;
    public GameObject hitPlayerEffectPrefab;

    public Slider healthBar;

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Make sure the health value never drops below 0
        currentHealth = Mathf.Max(currentHealth, 0);

        // Update health bar
        healthBar.value = currentHealth;


        //Instantiate particle effects
        if (hitPlayerEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitPlayerEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration); 
        }


        if (currentHealth <= 0)
        {
            Die();
        }

        
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

    //to heal the player
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // update health bar
        healthBar.value = currentHealth;

        Debug.Log("Player Health Increased by: " + healAmount);
    }
    
}
