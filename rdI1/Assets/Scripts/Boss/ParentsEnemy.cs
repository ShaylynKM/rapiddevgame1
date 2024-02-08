using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentsEnemy : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    private Vector2 moveDirection;

    public GameObject naggingJokePrefab; 
    

    public float initialSpeed = 2.0f;
    public float maxSpeed = 6.0f;
    public float speedIncreaseInterval = 20.0f; 
    private float speedIncreaseTimer = 0;
    private float currentSpeed;

    public float attackInterval = 3.0f; 
    private float attackTimer;

    public Transform playerTransform;

    void Start()
    {
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        currentSpeed = initialSpeed;
        InvokeRepeating(nameof(LaunchAttack), attackInterval, attackInterval);
    }

    void Update()
    {
        
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;


        speedIncreaseTimer += Time.deltaTime;
        if (speedIncreaseTimer >= speedIncreaseInterval && currentSpeed < maxSpeed)
        {
            currentSpeed += 1.0f;
            speedIncreaseTimer = 0;
        }
    }

    private void LaunchAttack()
    {
        Vector2 attackDirection = (playerTransform.position - transform.position).normalized;
        GameObject joke = Instantiate(naggingJokePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = joke.GetComponent<Rigidbody2D>();
        rb.velocity = attackDirection * currentSpeed;
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

        if (collision.CompareTag("Boundary"))
        {
            
            moveDirection = -moveDirection; 
        }
    }
}
