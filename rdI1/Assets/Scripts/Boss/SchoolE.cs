using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolE : MonoBehaviour
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

    private float circleAttackCooldown = 10f; 
    private float landmineAttackCooldown = 15f; 
    private float nextCircleAttackTime = 0f; 
    private float nextLandmineAttackTime = 0f;

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
            int skillChoice = Random.Range(0, 3); 
            switch (skillChoice)
            {
                case 0:
                    FireBullet();
                    break;
                case 1:
                    if (Time.time >= nextCircleAttackTime)
                    {
                        CircleAttack();
                        nextCircleAttackTime = Time.time + circleAttackCooldown;
                    }
                    break;
                case 2:
                    if (Time.time >= nextLandmineAttackTime)
                    {
                        LandmineAttack();
                        nextLandmineAttackTime = Time.time + landmineAttackCooldown;
                    }
                    break;
            }
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

    void CircleAttack()
    {
        Circle circleComponent = GetComponent<Circle>();
        if (circleComponent != null)
        {
            circleComponent.Activate();
            Debug.Log("Performing Circle Attack");
        }
    }

    void LandmineAttack()
    {
        Landmine landmineComponent = GetComponent<Landmine>();
        if (landmineComponent != null)
        {
            landmineComponent.Activate();
            Debug.Log("Performing Landmine Attack");
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
