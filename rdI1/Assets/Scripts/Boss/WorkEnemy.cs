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
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
        currentProjectileSpeed = initialProjectileSpeed; 
       
        StartCoroutine(SpawnProjectileCycle());
    }

    void Update()
    {
        MoveAroundWaypoints();

        currentProjectileSpeed = Mathf.Min(currentProjectileSpeed + speedIncreaseRate * Time.deltaTime, maxProjectileSpeed);
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
