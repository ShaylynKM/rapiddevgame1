using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject badJokePrefab;
    public GameObject goodJokePrefab;
    public GameObject hintPrefab;// Prefab for the hint image

    public float moveSpeed = 2.0f; // 
    private bool movingRight = true; 
    public float moveDistance = 5.0f;

    private Vector2 startPosition;

    public float initialSpeed = 3.0f;
    public float maxSpeed = 10.0f;
    public float speedIncreaseInterval = 100.0f;
    private float speedIncreaseTimer = 0;
    private float currentSpeed;

    public float attackInterval = 2.0f;
    public float minAttackInterval = 0.5f;
    public float intervalDecreaseRate = 0.5f;

    public Transform playerTransform;// Player's Transform
    public Transform attackPositionsParent;// Empty GameObject that contains four 

    private Transform[] attackPositions; // Array for the four child positions
    private float timer;

    void Start()
    {
        startPosition = transform.position;

        // Retrieve the four child positions
        attackPositions = new Transform[attackPositionsParent.childCount];
        for (int i = 0; i < attackPositionsParent.childCount; i++)
        {
            attackPositions[i] = attackPositionsParent.GetChild(i);
        }

        currentSpeed = initialSpeed;
    }

    void Update()
    {
        MoveBoss();

        timer += Time.deltaTime;
        speedIncreaseTimer += Time.deltaTime;

        if (speedIncreaseTimer >= speedIncreaseInterval && currentSpeed < maxSpeed)
        {
            currentSpeed += 1.0f;
            speedIncreaseTimer = 0;
        }

        if (attackInterval > minAttackInterval)
        {
            attackInterval -= intervalDecreaseRate * Time.deltaTime;
            attackInterval = Mathf.Max(attackInterval, minAttackInterval);
        }

        if (timer >= attackInterval)
        {
            StartCoroutine(PrepareAndAttack());
            timer = 0;
        }
    }

    void MoveBoss()
    {
        if (movingRight)
        {
            if (transform.position.x < startPosition.x + moveDistance)
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                movingRight = false; 
            }
        }
        else
        {
            if (transform.position.x > startPosition.x - moveDistance)
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                movingRight = true; 
            }
        }
    }

    IEnumerator PrepareAndAttack()
    {
        int randomIndex = Random.Range(0, attackPositions.Length);
        Transform chosenPosition = attackPositions[randomIndex];

        
        GameObject hint = Instantiate(hintPrefab, chosenPosition.position, Quaternion.identity);

        // Wait for Jerry before launching the joke
        yield return new WaitForSeconds(1f); 

        Destroy(hint);

       
        Vector2 attackDirection = (playerTransform.position - chosenPosition.position).normalized;
        GameObject jokePrefab = Random.Range(0f, 1f) < 0.7f ? badJokePrefab : goodJokePrefab;
        GameObject joke = Instantiate(jokePrefab, chosenPosition.position, Quaternion.identity);
        Rigidbody2D rb = joke.GetComponent<Rigidbody2D>();
        rb.velocity = attackDirection * currentSpeed; 
    }
}