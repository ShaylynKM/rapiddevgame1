using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;  
    public float projectileSpeed = 10.0f; 
    public float shootDistance = 4f;

    private Vector2 shootDirection = Vector2.up; // Default shooting direction is upward

    void Update()
    {
        // Detect player's rotation
        if (Input.GetKeyDown(KeyCode.A)) // Rotate left
        {
            shootDirection = Vector2.left;
            RotatePlayer(shootDirection);
        }
        else if (Input.GetKeyDown(KeyCode.D)) // Rotate right
        {
            shootDirection = Vector2.right;
            RotatePlayer(shootDirection);
        }
        else if (Input.GetKeyDown(KeyCode.W)) // Rotate upward
        {
            shootDirection = Vector2.up;
            RotatePlayer(shootDirection);
        }
        else if (Input.GetKeyDown(KeyCode.S)) // Rotate downward
        {
            shootDirection = Vector2.down;
            RotatePlayer(shootDirection);
        }

        // Detect shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot(shootDirection);
        }
    }

    void Shoot(Vector2 direction)
    {
        // a projectile and set its velocity
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;

        // Set the bullet's rotation to face the correct direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Destroy the bullet after it travels the shoot distance
        Destroy(projectile, shootDistance / projectileSpeed);
    }

    void RotatePlayer(Vector2 direction)
    {
        // Set the player's rotation to face the correct direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}