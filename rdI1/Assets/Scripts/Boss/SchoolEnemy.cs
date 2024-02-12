using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolEnemy : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    private int currentTargetIndex = 0;

    public Transform[] cornerPoints;

    public GameObject bulletPrefab;
    public Transform firePoint; 
    public Transform player; 
    public float initialAttackInterval = 2.0f;
    public float minAttackInterval = 0.5f;
    public float intervalDecreaseRate = 0.1f; 
    private float currentAttackInterval; 
    private float attackTimer; 

    void Start()
    {
        currentAttackInterval = initialAttackInterval;
    }


    void Update()
    {
        MoveTowardsCorner();
        attackTimer += Time.deltaTime;

        
        if (attackTimer >= currentAttackInterval)
        {
            FireBullet();
            attackTimer = 0f;

            currentAttackInterval = Mathf.Max(minAttackInterval, currentAttackInterval - intervalDecreaseRate);
        }
    }

    void MoveTowardsCorner()
    {
        if (cornerPoints.Length == 0) return;
        
        Transform targetPoint = cornerPoints[currentTargetIndex];
        
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
      
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % cornerPoints.Length;
        }
    }

    void FireBullet()
    {
        if (player == null) return; 

        Vector2 direction = player.position - firePoint.position; 
        direction.Normalize(); 

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity); 
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 5f; 
        }
    }

        private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }


    }
}
