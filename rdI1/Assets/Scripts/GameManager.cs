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

    public bool CanMove { get; private set; }
    public bool CanShoot { get; private set; }
    public bool CanFreeze { get; private set; }

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
                UpdatePartyAbilities();
            }
            else if (partySceneTimer > 120f && currentStage == 2)
            {
                currentStage = 3;
                UpdatePartyAbilities();
            }
        }
    }

    private void UpdatePartyAbilities()
    {
        switch (currentStage)
        {
            case 1: //when i look at this, i have no idea which level this is. If you tell me I may forget later. If it was en enum value, I wouldn't be able to forget, since it would be Stage.Home, Stage.Job, etc
                CanMove = true;
                CanFreeze = false;
                CanShoot = false;
                break;
            case 2:
                CanMove = true;
                CanFreeze = true;
                CanShoot = false;
                break;
            case 3:
                CanMove = true;
                CanFreeze = true;
                CanShoot = true;
                break;
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

        SetInitialPlayerAbilities();
    }

    private void SetInitialPlayerAbilities()
    {
       
        CanMove = false;
        CanShoot = false;
        CanFreeze = false;

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
        SetAbilitiesBasedOnScene(scene.name);

        if (scene.name== "Party")
        {
            partySceneTimer = 0f;
            currentStage = 1; //magic numbers: bad
            UpdatePartyAbilities();
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
        else
        {
            DialogueTrigger dialogueTrigger = FindObjectOfType<DialogueTrigger>();
            if (dialogueTrigger != null)
            {
                dialogueTrigger.TriggerOutroDialogue();

                yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

                dialogueTrigger.OnOutroDialogueComplete -= OnOutroDialogueComplete;

                LoadNextLevel();
            }
        }
    }

    private void OnOutroDialogueComplete()
    {
        LoadNextLevel();
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
