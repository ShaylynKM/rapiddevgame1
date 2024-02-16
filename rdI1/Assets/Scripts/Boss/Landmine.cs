using System.Collections;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    public GameObject bombPrefab; 
    public GameObject warningPrefab; 
    public Transform playerTransform;
    public int bombCount = 6; 
    public float spawnDistance = 1f; 
    public float warningDuration = 1f; 
    public float bombFuseTime = 1f; 
    public float spawnInterval = 10f; 

    void Start()
    {
        InvokeRepeating("SpawnBombsAroundPlayer", 5f, spawnInterval);
    }

    void SpawnBombsAroundPlayer()
    {
        for (int i = 0; i < bombCount; i++)
        {
            float angle = i * (360f / bombCount) * Mathf.Deg2Rad;
            Vector2 spawnPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnDistance + (Vector2)playerTransform.position;
            GameObject warning = Instantiate(warningPrefab, spawnPos, Quaternion.identity);
            StartCoroutine(DelayedBombSpawn(warning, spawnPos));
        }
    }

    IEnumerator DelayedBombSpawn(GameObject warning, Vector2 spawnPos)
    {
        
        StartCoroutine(FlashWarning(warning.GetComponent<SpriteRenderer>()));
        yield return new WaitForSeconds(warningDuration);

        StopCoroutine(FlashWarning(warning.GetComponent<SpriteRenderer>())); 
        Destroy(warning);

        GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        StartCoroutine(BombFuse(bomb));
    }


    IEnumerator FlashWarning(SpriteRenderer spriteRenderer)
    {
        float endTime = Time.time + warningDuration;
        while (Time.time < endTime)
        {
            
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f); 
        }
        spriteRenderer.enabled = true;
    }


    IEnumerator BombFuse(GameObject bomb)
    {
        yield return new WaitForSeconds(bombFuseTime);
        
        bomb.GetComponent<Bomb>().Explode();
        
        Destroy(bomb);

    }
}


