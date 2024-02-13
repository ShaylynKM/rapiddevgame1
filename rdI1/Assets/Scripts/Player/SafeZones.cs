using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZones : MonoBehaviour
{
    public float anxietyThreshold = 0.8f;
    public float hideDuration = 5f;
    public GameObject[] hidingSpots; // Array to store hiding spots

    private AnxietyMeter anxietyMeter;
    private PlayerHealth playerHealth; // Reference to the PlayerHealth script
    private GameObject activeSpot; // Currently active hiding spot

    void Start()
    {
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

            if (currentFill >= anxietyThreshold)
            {
                SetHidingSpotActive(true);
            }
            else
            {
                SetHidingSpotActive(false);
                SetRandomHidingSpot();
            }
        }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hello");
        GameObject otherObject = other.gameObject;

        if (otherObject == playerHealth.gameObject)
        {
            if (playerHealth.IsHiding())
            {
                Debug.Log("Player is already hiding");
                return;
            }

            if (activeSpot.activeSelf)
            {
                Debug.Log("Player entered active hiding area");
                playerHealth.SetHiding(true);
            }
            else
            {
                Debug.Log("Player entered hiding area, but the spot is not active");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;

        if (otherObject == playerHealth.gameObject)
        {
            Debug.Log("Player exited hiding area");
            if (playerHealth != null)
            {
                playerHealth.SetHiding(false);
            }
        }
    }
}
