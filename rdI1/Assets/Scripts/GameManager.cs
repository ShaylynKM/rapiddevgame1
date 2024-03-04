using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GamePhaseSO currentPhase;
    private float phaseTimer = 0f;

    public GameObject winScreen;
   
    public GameObject anxietyMeterPrefab;
    private GameObject anxietyMeterInstance;

    public bool CanMove { get; private set; }
    public bool CanShoot { get; private set; }
    public bool CanFreeze { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetPhase(currentPhase);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Party")
        {
            phaseTimer += Time.deltaTime;
            if (phaseTimer >= currentPhase.phaseDuration)
            {
                if (currentPhase.nextPhase != null)
                {
                    SetPhase(currentPhase.nextPhase);
                }
                
            }
        }
    }

    private void SetPhase(GamePhaseSO newPhase)
    {
        currentPhase = newPhase;
        phaseTimer = 0f;

        AudioManager.Instance.PlayMusic(currentPhase.musicClip);

        UpdatePlayerAbilities(currentPhase.canMove, currentPhase.canShoot, currentPhase.canFreeze);

        if (currentPhase.anxietyMeterPrefab != null && anxietyMeterInstance == null)
        {
            anxietyMeterInstance = Instantiate(currentPhase.anxietyMeterPrefab);
           
        }

        if (currentPhase.musicClip != null)
        {
            AudioManager.Instance.PlayMusic(currentPhase.musicClip);
        }


    }


    private void UpdatePlayerAbilities(bool canMove, bool canShoot, bool canFreeze)
    {
        CanMove = canMove;
        CanShoot = canShoot;
        CanFreeze = canFreeze;

    }
  
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
       
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
       
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetAbilitiesBasedOnScene(scene.name);
       

        int sceneIndex = scene.buildIndex - 1;

        GamePhaseSO phaseConfig = LoadPhaseConfigForScene(scene.name);
        if (phaseConfig != null)
        {
            SetPhase(phaseConfig);
        }
       

    }

   
    GamePhaseSO LoadPhaseConfigForScene(string sceneName)
    {
        string path = "LevelsSettings/"; 
        switch (sceneName)
        {
            case "Home":
                path += "Home";
                break;
            case "School":
                path += "School";
                break;
            case "Job":
                path += "Job";
                break;
            case "Party":
                path += "Party1";
                break;
           
        }

        GamePhaseSO phaseConfig = Resources.Load<GamePhaseSO>(path);
        if (phaseConfig == null)
        {
            Debug.LogError("Failed " + path);
        }
        return phaseConfig;
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

}
