using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 15;
    private int currentHealth;

    public GameObject gameOverMenu;

    public GameObject hitPlayerEffectPrefab;

    public Slider healthBar;

    public GradeSystem gradeSystem;

    private bool isHiding = false;

    public Animator animator;

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void SetHiding(bool hiding)
    {
        isHiding = hiding;
    }

    public bool IsHiding()
    {
        return isHiding;
    }

    public void TakeDamage(int damage)
    {
        if (!isHiding)
        {
            currentHealth -= damage;
            AudioManager.Instance.Play(7, "hurt", false);

            // Make sure the health value never drops below 0
            currentHealth = Mathf.Max(currentHealth, 0);

            // Update health bar
            healthBar.value = currentHealth;

            // Instantiate particle effects
            if (hitPlayerEffectPrefab != null)
            {
                GameObject effect = Instantiate(hitPlayerEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
            }

            animator.Play("Playe_Hurt");

            gradeSystem.CounterTakeHit();

            if (currentHealth <= 0)
            {
                StartCoroutine(Die());
            }

            Debug.Log("Player Health Decreased by: " + damage);
        }
    }

    

    IEnumerator Die()
    {
        Debug.Log("die");
        if (currentHealth <= 0)
        {
            
            AudioManager.Instance.Play(5, "playerKill", false);
            Debug.Log("Player Died");
            animator.Play("Playe_Death");
            yield return new WaitForSeconds(3);
            gameOverMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

   
    private void GameOver()
    {
        Debug.Log("Game Over");
        
    }


    //to heal the player
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        AudioManager.Instance.Play(6, "heal", false);

        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // update health bar
        healthBar.value = currentHealth;

        Debug.Log("Player Health Increased by: " + healAmount);
    }
    
}
