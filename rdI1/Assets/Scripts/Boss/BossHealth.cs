using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    private int currentHealth;
    private SpriteRenderer spriteRenderer; 
    public Color hurtColor = Color.red;
    private Color originalColor; 
    public float colorChangeDuration = 0.5f;

    public GameObject winScreen;
    public Slider healthBar; 


    void Start()
    {
        currentHealth = maxHealth;

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Save original color
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Boss current health is" + currentHealth);

        //Make sure the health value never drops below 0
        currentHealth = Mathf.Max(currentHealth, 0);

        // Update health bar
        healthBar.value = currentHealth;

        // Update boss health UI here

        if (currentHealth <= 0)
        {
            Defeated();
            Debug.Log("Player win! ");
        }
    }

   

    void Win()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void TriggerColorChange()
    {
        StartCoroutine(ChangeColor());
    }


    private IEnumerator ChangeColor()
    {
        spriteRenderer.color = hurtColor; // change color
        yield return new WaitForSeconds(colorChangeDuration); // wait for seconds
        spriteRenderer.color = originalColor; // back original color
    }

    private void Defeated()
    {
        Debug.Log("Boss Defeated");
        Destroy(gameObject);
        ShowWinScreen();
    }

    private void ShowWinScreen()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
