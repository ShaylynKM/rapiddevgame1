using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZones : MonoBehaviour
{
    public float anxietyThreshold = 0.8f;
    public float hideDuration = 5f;
    public GameObject[] hidingSpots; 

    private AnxietyMeter anxietyMeter;
    private PlayerHealth playerHealth; 
    public GameObject activeSpot;
    private SpriteRenderer activeSpotRenderer; 

    private List<SpriteRenderer> safeZoneRenderers; 

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

        safeZoneRenderers = new List<SpriteRenderer>();

        foreach (GameObject spot in hidingSpots)
        {
            SpriteRenderer renderer = spot.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                safeZoneRenderers.Add(renderer);
            }
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
                playerHealth.gameObject.SetActive(true);
            }
        }
    }

    void SetHidingSpotActive(bool isActive)
    {
        float targetOpacity = isActive ? 1f : 0.3f;

        foreach (SpriteRenderer renderer in safeZoneRenderers)
        {
            if (renderer == activeSpotRenderer)
            {
                Color newColor = renderer.color;
                newColor.a = targetOpacity;
                renderer.color = newColor;
            }
            else
            {
                Color newColor = renderer.color;
                newColor.a = 0.3f;
                renderer.color = newColor;
            }
        }
    }

    void SetRandomHidingSpot()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        int randomIndex = Random.Range(0, hidingSpots.Length);
        activeSpot = hidingSpots[randomIndex];
        activeSpotRenderer = activeSpot.GetComponent<SpriteRenderer>();
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
