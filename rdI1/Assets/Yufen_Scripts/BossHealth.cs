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
        spriteRenderer.color = hurtColor; // 改变颜色
        yield return new WaitForSeconds(colorChangeDuration); // 等待一段时间
        spriteRenderer.color = originalColor; // 恢复原始颜色
    }

    private void Defeated()
    {
     
        Debug.Log("Boss Defeated");
        Destroy(gameObject);
        
    }
}
