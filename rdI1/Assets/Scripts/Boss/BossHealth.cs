using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private SpriteRenderer spriteRenderer; 
    public Color hurtColor = Color.red;
    private Color originalColor; 
    public float colorChangeDuration = 0.5f;

    void Start()
    {
        currentHealth = maxHealth;

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

        // Update boss health UI here

        if (currentHealth <= 0)
        {
            Defeated();
            Debug.Log("Player win! ");
        }
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
        
    }
}
