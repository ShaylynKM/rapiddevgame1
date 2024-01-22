using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffectPrefab; // Prefab for the collision particle effect

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check collision withenemies
        if (other.gameObject.CompareTag("Jokes"))
        {
            BossHealth bossHealth = FindObjectOfType<BossHealth>(); // find BossHealth
            if (bossHealth != null)
            {
                bossHealth.TriggerColorChange();
            }

            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, 1f);

            Destroy(gameObject);
        }
    }
}
