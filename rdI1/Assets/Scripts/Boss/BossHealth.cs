using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; 
    public Color hurtColor = Color.red;
    private Color originalColor; 
    public float colorChangeDuration = 0.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Save original color
        }
    }
}
