using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winCanvas;

    public bool CanMove { get; private set; }
    public bool CanShoot { get; private set; }
    public bool CanFreeze { get; private set; }

    public float baseSurvivalTime = 30f; 
    public float[] levelTimeMultipliers = { 1, 2, 3, 4 }; 

    private int currentLevelIndex = 0; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        if (winCanvas != null)
        {
            winCanvas.SetActive(false); 
        }
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
        float survivalTime = baseSurvivalTime * levelTimeMultipliers[currentLevelIndex];
        StartCoroutine(CountdownToNextLevel(survivalTime));
    }

    IEnumerator CountdownToNextLevel(float time)
    {
        yield return new WaitForSeconds(time);

        if (currentLevelIndex >= levelTimeMultipliers.Length - 1)
        {
            ShowWinScreen();
        }
        else
        {
            currentLevelIndex++;
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

    private void ShowWinScreen()
    {
        if (winCanvas != null)
        {
            winCanvas.SetActive(true); 
        }
    }
}
