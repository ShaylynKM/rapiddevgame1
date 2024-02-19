using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Circle : MonoBehaviour
{
    public GameObject bombPrefab;
    public GameObject finalBombPrefab;
    public GameObject warningPrefab;
    public Transform playerTransform;
    public int bombCount = 6;
    public float spawnDistance = 1f;
    public float convergeSpeed = 2f;
    public float warningDuration = 1f;
    public float bombFuseTime = 1f;
    public float spawnInterval = 10f;

    private List<GameObject> bombs = new List<GameObject>();

    void Start()
    {
        InvokeRepeating("SpawnBombsAroundPlayer", 4f, spawnInterval);
    }

    public void Activate()
    {
        SpawnBombsAroundPlayer();
        
    }

    void SpawnBombsAroundPlayer()
    {
        Vector2 centerPoint = playerTransform.position;
        for (int i = 0; i < bombCount; i++)
        {
            float angle = i * (360f / bombCount) * Mathf.Deg2Rad;
            Vector2 spawnPos = centerPoint + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnDistance;
            GameObject warning = Instantiate(warningPrefab, spawnPos, Quaternion.identity);
            StartCoroutine(DelayedBombSpawn(warning, spawnPos, centerPoint));
        }
    }

    IEnumerator DelayedBombSpawn(GameObject warning, Vector2 spawnPos, Vector2 centerPoint)
    {
        yield return new WaitForSeconds(warningDuration);
        Destroy(warning);

        GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        StartCoroutine(MoveBombTowardsCenter(bomb, centerPoint));
    }

    IEnumerator MoveBombTowardsCenter(GameObject bomb, Vector2 centerPoint)
    {
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        GameObject finalBombInstance = null;

        while (Vector2.Distance(bomb.transform.position, centerPoint) > 0.1f)
        {
            Vector2 direction = (centerPoint - new Vector2(bomb.transform.position.x, bomb.transform.position.y)).normalized;
            rb.velocity = direction * convergeSpeed;
            yield return null;
        }

        rb.velocity = Vector2.zero; 

        bombs.Add(bomb);

        if (bombs.Count >= bombCount)
        {
            foreach (var b in bombs)
            {
                Destroy(b); 
            }
            bombs.Clear(); 

          
            finalBombInstance = Instantiate(finalBombPrefab, centerPoint, Quaternion.identity);
        }

       
        yield return new WaitForSeconds(bombFuseTime);

      
        if (finalBombInstance != null)
        {
            finalBombInstance.GetComponent<Bomb>().Explode();
            Destroy(finalBombInstance, 2f);
        }
    }

}
