using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class LevelsManager : MonoBehaviour
{
    public GameObject winScreen;

    public GameObject healPrefab;
    public Transform[] healSpawnPoints;

    public int healItemCount = 3;
    public float healSpawnInterval = 10f;
    public float healItemLifetime = 5f;

    public float levelDuration = 30f;
    public TextMeshProUGUI countdownText;

    private void Start()
    {
        StartHealItemSpawn();
        StartCoroutine(Countdown(levelDuration)); 
    }

    private void StartHealItemSpawn()
    {
        StartCoroutine(SpawnHealItems());
    }

    private List<GameObject> spawnedHealItems = new List<GameObject>();

    IEnumerator SpawnHealItems()
    {
        for (int i = 0; i < healItemCount; i++)
        {
            yield return new WaitForSeconds(healSpawnInterval);

            Vector3 spawnPosition = GetSpawnPosition();

            GameObject healItem = Instantiate(healPrefab, spawnPosition, Quaternion.identity);
            spawnedHealItems.Add(healItem);

            StartCoroutine(DespawnHealItem(healItem, healItemLifetime));
        }
    }

    IEnumerator DespawnHealItem(GameObject healItem, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        if (healItem != null)
        {
            Destroy(healItem);
            spawnedHealItems.Remove(healItem);
        }
    }


    Vector3 GetSpawnPosition()
    {
        if ((SceneManager.GetActiveScene().name == "Job" || SceneManager.GetActiveScene().name == "Party") && healSpawnPoints.Length > 0)
        {
            Transform selectedSpawnPoint = healSpawnPoints[Random.Range(0, healSpawnPoints.Length)];
            return selectedSpawnPoint.position;
        }
        else
        {
            return new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
        }
    }

    IEnumerator Countdown(float time)
    {
        while (time > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = $"Time Left: {time}";
            }
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }

       
        TimeUp();
    }

    void TimeUp()
    {
        StopAllCoroutines(); // Stop all running coroutines, including SpawnHealItems

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            ShowWinScreen();
            Debug.Log("You've completed all levels!");
        }
    }

    private void ShowWinScreen()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
