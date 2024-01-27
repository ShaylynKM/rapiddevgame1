using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverMenuDisplay;

    [SerializeField]
    private string mainMenuSceneName = "MainMenuGym"; 

    void Start()
    {
        gameOverMenuDisplay.SetActive(false);

        //AudioManager.Instance.Play(0, "bg", true);

    }

    
    public void ShowGameOverMenu()
    {
        gameOverMenuDisplay.SetActive(true);
        Time.timeScale = 0f; 
    }

    #region Button Actions

    public void OnRestartPress()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    public void OnMainMenuPress()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName); 
    }

    public void OnExitPress()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); 
    }

    #endregion
}
