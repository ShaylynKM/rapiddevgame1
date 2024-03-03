using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
enum Stage
{
    Home, School, Job, Party
}
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameObject winScreen;
    public TextMeshProUGUI countdownText;

    public float baseSurvivalTime = 30f;
    public float[] levelSurvivalTimes;

    public GameObject healPrefab;
    public Transform[] healSpawnPoints;

    public int healItemCount = 3;
    public float healSpawnInterval = 10f;

    private float partySceneTimer = 0f;
    private int currentStage = 1;



    private void Update()
    {
        
        if (SceneManager.GetActiveScene().name == "Party")
        {
            partySceneTimer += Time.deltaTime;

            if (partySceneTimer > 60f && currentStage == 1) //magic numbers: bad. Each level should keep track of its own timer
            {
                currentStage = 2;
              
            }
            else if (partySceneTimer > 120f && currentStage == 2)
            {
                currentStage = 3;
              
            }
        }
    }

   


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



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
     

        if (scene.name== "Party")
        {
            partySceneTimer = 0f;
            currentStage = 1; //magic numbers: bad
            
        }

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

            Vector3 spawnPosition;
            if (SceneManager.GetActiveScene().name == "Job" && healSpawnPoints.Length > 0)
            {
                
                Transform selectedSpawnPoint = healSpawnPoints[Random.Range(0, healSpawnPoints.Length)];
                spawnPosition = selectedSpawnPoint.position;
            }

            else if (SceneManager.GetActiveScene().name == "Party" && healSpawnPoints.Length > 0)
            {
                Transform selectedSpawnPoint = healSpawnPoints[Random.Range(0, healSpawnPoints.Length)];
                spawnPosition = selectedSpawnPoint.position;
            }

            else
            {
                
                spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0); //magic numbers: bad.
            }

            Instantiate(healPrefab, spawnPosition, Quaternion.identity);
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

    IEnumerator CountdownToNextLevel(float time)
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

        Debug.Log("Countdown completed.");

        if (countdownText != null)
        {
            countdownText.text = "";
        }

        if (SceneManager.GetActiveScene().buildIndex == levelSurvivalTimes.Length)
        {
            Defeated();
            ShowWinScreen();
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
