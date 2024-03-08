using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public GameObject badJokePrefab;
    //public GameObject goodJokePrefab;
    public GameObject hintPrefab;// Prefab for the hint image

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

    [SerializeField]
    private AnimationClip idleAnim;

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

    //public void OnSafezoneDespawned()
    //{
    //    StartCoroutine(DelayedAttackAdjustment());
    //    Debug.Log("Lowering attacks");
    //}

    //IEnumerator DelayedAttackAdjustment()
    //{
    //    attackInterval += 0.5f; // Increase attack interval
    //    currentSpeed -= 1.0f;   // Decrease attack speed
    //    yield return new WaitForSeconds(2f);
    //}


    IEnumerator PrepareAndAttack()
    {
        int randomIndex = Random.Range(0, attackPositions.Length);
        Transform chosenPosition = attackPositions[randomIndex];

        
        GameObject hint = Instantiate(hintPrefab, chosenPosition.position, Quaternion.identity);

        // Wait for Jerry before launching the joke
        yield return new WaitForSeconds(1f); 

        Destroy(hint);

       
        Vector2 attackDirection = (playerTransform.position - chosenPosition.position).normalized;
        GameObject jokePrefab = badJokePrefab;
        GameObject joke = Instantiate(jokePrefab, chosenPosition.position, Quaternion.identity);
        Rigidbody2D rb = joke.GetComponent<Rigidbody2D>();
        rb.velocity = attackDirection * currentSpeed; 
    }
}