using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public SceneConfigurations sceneConfigs;

    public GamePhaseSO currentPhase;
    private float phaseTimer = 0f;

    public GameObject winScreen;
   
    public GameObject anxietyMeterPrefab;

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

        if (currentPhase.anxietyMeterPrefab != null)
        {
            anxietyMeterPrefab.SetActive(true);
           
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
       


        int sceneIndex = scene.buildIndex - 1;

        GamePhaseSO phaseConfig = LoadPhaseConfigForScene(scene.name);
        if (phaseConfig != null)
        {
            SetPhase(phaseConfig);
        }
       

    }


    GamePhaseSO LoadPhaseConfigForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Home":
                return sceneConfigs.homeConfig;
            case "School":
                return sceneConfigs.schoolConfig;
            case "Job":
                return sceneConfigs.jobConfig;
            case "Party":
                return sceneConfigs.partyConfig;
          
            default:
               
                return null;
        }
    }




}
