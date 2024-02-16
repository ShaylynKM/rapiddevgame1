using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionEffectPrefab; 

    public void Explode()
    {
        if (explosionEffectPrefab)
        {
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            
            Destroy(explosionEffect, 1f); 
        }
        Debug.Log("Bomb exploded!");
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Original logic for handling player collision
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); 
                
            }

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall"))
        {

            Destroy(gameObject);
        }
    }
}