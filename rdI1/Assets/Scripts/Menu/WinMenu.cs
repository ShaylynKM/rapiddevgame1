using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject winScreenDisplay;

    [SerializeField]
    private string mainMenuSceneName = "MainMenuGym";


    public Image gradeImage;
    
    void Start()
    {
        winScreenDisplay.SetActive(false);
    }


    public void ShowGameOverMenu()
    {
        winScreenDisplay.SetActive(true);
        Time.timeScale = 0f;
    }

    public void UpdateImage(string grade)
    {
        if (GradeSystem.gradeToImage.TryGetValue(grade, out string imageName))
        {
            Sprite sprite = Resources.Load<Sprite>(imageName);

            if (sprite != null)
            {
                gradeImage.sprite = sprite;
            }
            else
            {
                Debug.LogError($"Failed to load image for grade {grade}: {imageName}");
            }
        }
        else
        {
            Debug.LogWarning($"No image found for grade {grade}");
        }
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
        AudioManager.Instance.Stop(0);
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void OnExitPress()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
    #endregion
 }