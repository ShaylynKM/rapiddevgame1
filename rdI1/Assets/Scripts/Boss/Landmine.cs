using System.Collections;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    public GameObject bombPrefab;
    public GameObject warningPrefab;
    public float spawnAreaRadius = 5f; 
    public int bombCount = 6;
    public float warningDuration = 1f;
    public float bombFuseTime = 1f;
    public float spawnInterval = 10f;

    void Start()
    {
        InvokeRepeating("SpawnBombsRandomly", 15f, spawnInterval);
    }

    public void Activate()
    {
        SpawnBombsRandomly();
    }

    void SpawnBombsRandomly()
    {
        for (int i = 0; i < bombCount; i++)
        {
           
            Vector2 spawnPos = Random.insideUnitCircle * spawnAreaRadius;
            GameObject warning = Instantiate(warningPrefab, spawnPos, Quaternion.identity);
            StartCoroutine(DelayedBombSpawn(warning, spawnPos));
        }
    }

    IEnumerator DelayedBombSpawn(GameObject warning, Vector2 spawnPos)
    {
        StartCoroutine(FlashWarning(warning.GetComponent<SpriteRenderer>()));
        yield return new WaitForSeconds(warningDuration);

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
