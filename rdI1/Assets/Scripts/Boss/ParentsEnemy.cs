using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootDirection
{
    Up,
    Down,
    Left,
    Right
}

public class ParentsEnemy : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float attackSpeed = 5.0f;
    public GameObject badJokePrefab;
    public GameObject JokePrefab;
    //public GameObject goodJokePrefab;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    public Transform playerTransform;
    public float attackInterval = 2.0f;
    private float timer;

    public ShootDirection shootDirection = ShootDirection.Up;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }

    }

    void Update()
    {
        MoveAroundWaypoints();
        HandleAttack();
    }

    void MoveAroundWaypoints()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void HandleAttack()
    {
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            StartCoroutine(PrepareAndAttack());
            timer = 0;
        }
    }

    IEnumerator PrepareAndAttack()
    {
        
        Vector2 attackDirection;
        switch (shootDirection)
        {
            case ShootDirection.Up:
                attackDirection = Vector2.up;
                break;
            case ShootDirection.Down:
                attackDirection = Vector2.down;
                break;
            case ShootDirection.Left:
                attackDirection = Vector2.left;
                break;
            case ShootDirection.Right:
                attackDirection = Vector2.right;
                break;
            default:
                attackDirection = Vector2.up;
                break;
        }

        GameObject jokePrefab = Random.Range(0f, 1f) < 0.7f ? JokePrefab : badJokePrefab;
        GameObject joke = Instantiate(jokePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = joke.GetComponent<Rigidbody2D>();
        rb.velocity = attackDirection * attackSpeed;


        yield return new WaitForSeconds(0.1f);
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
