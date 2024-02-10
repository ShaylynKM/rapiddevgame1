using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobEnemy : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private Vector2 moveDirection;

    [SerializeField] private bool isFrozen = false;

    public GameObject badJokePrefab;
    public GameObject goodJokePrefab;
    public GameObject hintPrefab; // Prefab for the hint image

    public float initialSpeed = 3.0f;
    public float maxSpeed = 10.0f;
    public float speedIncreaseInterval = 100.0f;
    //private float speedIncreaseTimer = 0;
    private float currentSpeed;

    public float attackInterval = 2.0f;
    public float minAttackInterval = 0.5f;
    public float intervalDecreaseRate = 0.5f;

    public Transform playerTransform; // Player's Transform
    public Transform attackPositionsParent; // Empty GameObject that contains four child positions

    private Transform[] attackPositions; // Array for the four child positions
    private float timer;

    private Coroutine attackCoroutine;

    void Start()
    {
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    
        attackPositions = new Transform[attackPositionsParent.childCount];
        for (int i = 0; i < attackPositionsParent.childCount; i++)
        {
            attackPositions[i] = attackPositionsParent.GetChild(i);
        }
        currentSpeed = initialSpeed;
        StartAttackRoutine();
    }

    void Update()
    {

        if (isFrozen)
        {
            
            Debug.Log("Enemy is frozen and cannot move or attack.");
            return;
        }

        
        MoveEnemy();

       
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            StartCoroutine(AttackRoutine());
            timer = 0;
        }
    }

    void MoveEnemy()
    {
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
    }

    private void StartAttackRoutine()
    {
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (!isFrozen)
        {
            yield return new WaitForSeconds(attackInterval);
            LaunchAttack();
        }
    }

    private void LaunchAttack()
    {
        int randomIndex = Random.Range(0, attackPositions.Length);
        Transform chosenPosition = attackPositions[randomIndex];

        GameObject hint = Instantiate(hintPrefab, chosenPosition.position, Quaternion.identity);
        Destroy(hint, 1f); 

        Vector2 attackDirection = (playerTransform.position - chosenPosition.position).normalized;
        GameObject jokePrefab = Random.Range(0f, 1f) < 0.7f ? badJokePrefab : goodJokePrefab;
        GameObject joke = Instantiate(jokePrefab, chosenPosition.position, Quaternion.identity);
        Rigidbody2D rb = joke.GetComponent<Rigidbody2D>();
        rb.velocity = attackDirection * currentSpeed;
    }

    public void Freeze(float duration)
    {
        if (isFrozen) return;
        isFrozen = true;
        if (attackCoroutine != null) StopCoroutine(attackCoroutine); // Stop attacking
        StartCoroutine(UnfreezeAfterDelay(duration));
    }

    IEnumerator UnfreezeAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        StartAttackRoutine(); // Resume attacking
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Freeze(5f); // Freeze for 5 seconds when hit by a player bullet

        }
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }

        if (collision.CompareTag("Boundary"))
        {
           
            moveDirection = -moveDirection; 
        }
    }


}
