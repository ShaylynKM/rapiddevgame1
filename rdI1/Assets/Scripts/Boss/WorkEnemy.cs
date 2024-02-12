using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkEnemy : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    private bool movingRight = true;

    public GameObject bulletPrefab; 
    public Transform firePoint;
    public Transform player; 
    public float initialAttackInterval = 2.0f; 
    public float minAttackInterval = 0.5f; 
    public float intervalDecreaseRate = 0.1f; 
    private float currentAttackInterval; 
    private float attackTimer;

    public Transform leftBound;
    public Transform rightBound;

    void Start()
    {
        currentAttackInterval = initialAttackInterval; 
    }

    void Update()
    {
        
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x >= rightBound.position.x)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x <= leftBound.position.x)
            {
                movingRight = true;
            }
        }


        attackTimer += Time.deltaTime;

        if (attackTimer >= currentAttackInterval)
        {
            FireBullet();
            attackTimer = 0f;

         
            currentAttackInterval = Mathf.Max(minAttackInterval, currentAttackInterval - intervalDecreaseRate);
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
}

