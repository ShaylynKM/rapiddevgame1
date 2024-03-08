using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZones : MonoBehaviour
{
    public float anxietyThreshold = 0.8f;
    public float waitingTime = 8f;
    public float hideDuration = 5f;
    public float fadeDuration = 2f;
    public GameObject[] hidingSpots; // Array to store hiding spots

    public BossAI bossAI;
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
            SetSpriteAlpha(spot, 0f);
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
                StartCoroutine(ShowHidingSpotWithFade());
            }
            
        }
    }

    IEnumerator ShowHidingSpotWithFade()
    {
        SetHidingSpotActive(true);
        float timer = 0f;

        // Fade-in
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            SetSpriteAlpha(activeSpot, alpha);
            yield return null;
        }


        yield return new WaitForSeconds(waitingTime);

        // Fade-out
        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            SetSpriteAlpha(activeSpot, alpha);
            yield return null;
        }

        SetHidingSpotActive(false);
        SetRandomHidingSpot();
    }

    void SetSpriteAlpha(GameObject obj, float alpha)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(waitingTime);
        playerHealth.gameObject.SetActive(true);
        SetHidingSpotActive(false);
        SetRandomHidingSpot();

        //// Notify BossAI script that the safezone has despawned
        //GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        //if (boss != null)
        //{
        //    boss.SendMessage("OnSafezoneDespawned");
        //    Debug.Log("Safezone Despawned");
        //}
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
