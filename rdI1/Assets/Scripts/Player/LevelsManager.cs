using System.Collections;
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

    IEnumerator SpawnHealItems()
    {
        for (int i = 0; i < healItemCount; i++)
        {
            yield return new WaitForSeconds(healSpawnInterval);

            Vector3 spawnPosition = GetSpawnPosition();

            Instantiate(healPrefab, spawnPosition, Quaternion.identity);
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
