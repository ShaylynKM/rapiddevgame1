using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public GameObject projectilePrefab;
    public Transform[] spawnPoints;
    public float initialSpawnInterval = 1f;
    public float speedIncreaseInterval = 10f;
    public float speedIncreaseAmount = 0.5f;
    private float currentSpeed = 5f;
    private float nextSpeedIncreaseTime;

    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        nextSpeedIncreaseTime = Time.time + speedIncreaseInterval;
        StartCoroutine(SpawnProjectiles());
    }

    void Update()
    {
        MoveAroundWaypoints();
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

    IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Vector2 spawnPosition = transform.position + (Vector3)spawnPoints[randomIndex].localPosition;

            SpawnProjectile(spawnPosition);

            yield return new WaitForSeconds(initialSpawnInterval);

         
            if (Time.time >= nextSpeedIncreaseTime)
            {
                currentSpeed += speedIncreaseAmount;
                initialSpawnInterval = Mathf.Max(initialSpawnInterval - speedIncreaseAmount, 0.1f);
                nextSpeedIncreaseTime += speedIncreaseInterval;
            }
        }
    }

    void SpawnProjectile(Vector2 spawnPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * currentSpeed; 
    }
}
