using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobJoke : MonoBehaviour
{
    private float movementSpeed = 5f;
    public Transform playerTransform;

    public void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    void Update()
    {
        transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            // Original logic for handling player collision
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Inflict damage

            }

            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("Player"))
        {
            // Original logic for handling player collision
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
              
                playerHealth.TakeDamage(1); // Inflict damage
                
            }

            Destroy(gameObject);
        }

    }
}