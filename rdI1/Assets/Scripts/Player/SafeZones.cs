using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZones : MonoBehaviour
{
    public float anxietyThreshold = 0.8f;
    public float waitingTime = 8f;
    public float hideDuration = 5f;
    public GameObject[] hidingSpots; // Array to store hiding spots

    private AnxietyMeter anxietyMeter;
    private PlayerController playerController;
    private PlayerHealth playerHealth; // Reference to the PlayerHealth script
    private GameObject activeSpot; // Currently active hiding spot

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        anxietyMeter = FindObjectOfType<AnxietyMeter>();
        if (anxietyMeter == null)
        {
            Debug.LogError("AnxietyMeter component not found in the scene.");
        }

        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component not found in the scene.");
        }

        foreach (GameObject spot in hidingSpots)
        {
            spot.SetActive(false);
        }

        SetRandomHidingSpot();
    }

    void Update()
    {
        if (anxietyMeter != null && playerHealth != null && activeSpot != null)
        {
            float currentFill = anxietyMeter.currentFill;

            if (currentFill >= anxietyThreshold && !playerController.isFrozen)
            {
                SetHidingSpotActive(true);
                StartCoroutine("Delay");
            }
            
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(waitingTime);
        playerHealth.gameObject.SetActive(true);
        SetHidingSpotActive(false);
        SetRandomHidingSpot();
    }

    void SetHidingSpotActive(bool isActive)
    {
        if (activeSpot != null)
        {
            activeSpot.SetActive(isActive);
        }
    }

    void SetRandomHidingSpot()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        int randomIndex = Random.Range(0, hidingSpots.Length);
        activeSpot = hidingSpots[randomIndex];
    }

    public void PlayerEntered()
    {
        Debug.Log("hello");
        playerHealth.SetHiding(true);
        playerHealth.enabled = false;
    }

    public void PlayerExited()
    {
        Debug.Log("bye");
        playerHealth.enabled = true;
        playerHealth.SetHiding(false);
    }

}
