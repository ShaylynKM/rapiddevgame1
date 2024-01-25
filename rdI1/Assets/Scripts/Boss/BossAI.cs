using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public GameObject badJokePrefab; 
    public GameObject goodJokePrefab;
    public GameObject invincibleJokePrefab;

    public float initialSpeed = 3.0f; 
    public float maxSpeed = 10.0f; 
    public float speedIncreaseInterval = 100.0f; 
    private float speedIncreaseTimer = 0; 
    private float currentSpeed;

    public float attackInterval = 2.0f;
    public float minAttackInterval = 0.5f; 
    public float intervalDecreaseRate = 0.5f; 


    public Transform playerTransform; // Player's Transform
    public Transform attackPositionsParent; // Empty GameObject that contains four child positions

    private Transform[] attackPositions; // Array for the four child positions
    private float timer;

    void Start()
    {
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
        timer += Time.deltaTime;
        speedIncreaseTimer += Time.deltaTime;

        if (speedIncreaseTimer >= speedIncreaseInterval && currentSpeed < maxSpeed)
        {
            currentSpeed += 1.0f; 
            speedIncreaseTimer = 0;
            //Debug.Log("Current Speed: " + currentSpeed);
        }

        if (attackInterval > minAttackInterval)
        {
            attackInterval -= intervalDecreaseRate * Time.deltaTime;
            attackInterval = Mathf.Max(attackInterval, minAttackInterval);
            //Debug.Log("Current Attack Interval: " + attackInterval);
        }

        
        if (timer >= attackInterval)
        {
            Attack();
            timer = 0; 
        }
    }


    void Attack()
    {
        // Get the player's position
        Vector2 playerPosition = playerTransform.position;

        // Randomly select one of the child positions
        int randomIndex = Random.Range(0, attackPositions.Length);
        Transform chosenPosition = attackPositions[randomIndex];

        // Calculate the direction towards the player
        Vector2 attackDirection = (playerPosition - (Vector2)chosenPosition.position).normalized;

        // Randomly decide the probability of generating a bad joke over a good one
        float jokeTypeRandom = Random.Range(0f, 1f);
        
        bool shootInvincibleJoke = Random.Range(0f, 1f) < 0.05f; 

        
        GameObject selectedJokePrefab = shootInvincibleJoke ? invincibleJokePrefab : (jokeTypeRandom < 0.7f ? badJokePrefab : goodJokePrefab);

        GameObject joke = Instantiate(selectedJokePrefab, chosenPosition.position, Quaternion.identity);
        Rigidbody2D rb = joke.GetComponent<Rigidbody2D>();
        rb.velocity = attackDirection * currentSpeed;
    }
}
