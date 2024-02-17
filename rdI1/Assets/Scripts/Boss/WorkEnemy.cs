using System.Collections;
using UnityEngine;

public class WorkEnemy : MonoBehaviour
{
    public GameObject badJokePrefab;

    public float initialProjectileSpeed = 5f; 
    public float maxProjectileSpeed = 15f; 
    public float speedIncreaseRate = 0.2f;
    private float currentProjectileSpeed;

    public float spawnInterval = 1.0f;

    public float moveSpeed = 2f; 
    public float moveDistance = 5f; 

    private Vector2 startPosition;
    private bool movingRight = true;

    void Start()
    {
        currentProjectileSpeed = initialProjectileSpeed; 
        startPosition = transform.position;
        StartCoroutine(SpawnProjectileCycle());
    }

    void Update()
    {
        MoveEnemy();
       
        currentProjectileSpeed = Mathf.Min(currentProjectileSpeed + speedIncreaseRate * Time.deltaTime, maxProjectileSpeed);
    }

    void MoveEnemy()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x > startPosition.x + moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x < startPosition.x - moveDistance)
            {
                movingRight = true;
            }
        }
    }

    IEnumerator SpawnProjectileCycle()
    {
        while (true)
        {
            SpawnProjectile(transform.position);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnProjectile(Vector2 spawnPosition)
    {
        GameObject projectile = Instantiate(badJokePrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
       
        rb.velocity = Vector2.up * currentProjectileSpeed;
    }
}
