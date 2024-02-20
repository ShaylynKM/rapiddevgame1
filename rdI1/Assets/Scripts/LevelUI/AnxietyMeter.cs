using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnxietyMeter : MonoBehaviour
{
    public Transform arrowRect;
    public float fillSpeed = 0.1f;
    public float freezeTime = 5f;

    public float currentFill = 0f;
    private PlayerController playerController;

    public GameObject meterUI; 

   
    public void ActivateMeter()
    {
        meterUI.SetActive(true); 
        Time.timeScale = 1; 
    }

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController component not found in the scene.");
        }
    }

    void Update()
    {
        if (playerController != null && !playerController.isFrozen)
        {
            currentFill += fillSpeed * Time.deltaTime;
            currentFill = Mathf.Clamp01(currentFill); 

            float rotationAngle = currentFill * -180f; 
            arrowRect.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

            if (currentFill >= 1f)
            {
                playerController.FreezePlayer();
                Invoke("ResetAnxietyMeter", freezeTime);
            }
        }
    }

    private void ResetAnxietyMeter()
    {
        currentFill = 0f;
        arrowRect.rotation = Quaternion.identity;

        if (playerController != null)
        {
            playerController.UnfreezePlayer();
        }
    }

    public float GetCurrentFill()
    {
        return currentFill;
    }
}
