using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameObject winScreen;
    public TextMeshProUGUI countdownText;

    public bool CanMove { get; private set; }
    public bool CanShoot { get; private set; }
    public bool CanFreeze { get; private set; }

    public float baseSurvivalTime = 30f;
    public float[] levelSurvivalTimes;

    public GameObject healPrefab;
    public int healItemCount = 3;
    public float healSpawnInterval = 10f;

    public void ShowCountdownTimer()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(true);
    }

    
    public void HideCountdownTimer()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
           // Destroy(gameObject);
        }

        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueComplete += StartCountdownAfterDialogue;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueComplete -= StartCountdownAfterDialogue;
        }
    }

    private void StartCountdownAfterDialogue()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (sceneIndex >= 0 && sceneIndex < levelSurvivalTimes.Length)
        {
            StartCoroutine(CountdownToNextLevel(levelSurvivalTimes[sceneIndex]));
        }
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject TextTime = GameObject.Find("TextTime");
        if (TextTime != null)
        {
            countdownText = TextTime.GetComponent<TextMeshProUGUI>();
        }

        int sceneIndex = scene.buildIndex - 1;
        if (sceneIndex >= 0 && sceneIndex < levelSurvivalTimes.Length)
        {
            StartCoroutine(CountdownToNextLevel(levelSurvivalTimes[sceneIndex]));
        }

        StartHealItemSpawn();

        SetAbilitiesBasedOnScene(scene.name);
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
          
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            Instantiate(healPrefab, spawnPosition, Quaternion.identity);
        }
    }


    IEnumerator CountdownToNextLevel(float time)
    {
        while (DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive)
        {
            yield return null;
        }

        while (time > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = $"Time Left: {time}";
            }
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }
        
        
        if (countdownText != null)
        {
            countdownText.text = "";
        }
        

        if (SceneManager.GetActiveScene().buildIndex == levelSurvivalTimes.Length)
        {
            Defeated();
            ShowWinScreen();
        }
        else
        {
            LoadNextLevel();
        }
    }


    private void SetAbilitiesBasedOnScene(string sceneName)
    {
        CanMove = CanShoot = CanFreeze = false;

        switch (sceneName)
        {
            case "Home":
                CanMove = true;
                break;
            case "School":
                CanMove = CanShoot = true;
                break;
            case "Job":
                CanMove = CanFreeze = true;
                break;
            case "Party":
                CanMove = CanShoot = CanFreeze = true;
                break;
        }
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            ShowWinScreen();
        }
    }

    private void Defeated()
    {
        Debug.Log("winnn");
        //AudioManager.Instance.Play(4, "bossKill", false);
        
        ShowWinScreen();
    }

    private void ShowWinScreen()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
