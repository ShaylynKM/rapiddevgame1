using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public GameObject badJokePrefab; 
    public GameObject goodJokePrefab; 
    public float attackInterval = 2.0f; 
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
    }

    void Update()
    {
        timer += Time.deltaTime;

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
        GameObject jokePrefab = jokeTypeRandom < 0.7f ? badJokePrefab : goodJokePrefab; // 70% chance to generate a bad joke

        // Create and launch the joke
        GameObject joke = Instantiate(jokePrefab, chosenPosition.position, Quaternion.identity);
        Rigidbody2D rb = joke.GetComponent<Rigidbody2D>();
        rb.velocity = attackDirection * Random.Range(3.0f, 5.0f); // Random speed
    }
}
