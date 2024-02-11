using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingObject : MonoBehaviour
{
    public float anxietyThreshold = 0.8f; 
    public float hideDuration = 5f;
    public GameObject hidingObject;

    private AnxietyMeter anxietyMeter;

    void Start()
    {
        anxietyMeter = FindObjectOfType<AnxietyMeter>();
        if (anxietyMeter == null)
        {
            Debug.LogError("AnxietyMeter component not found in the scene.");
        }

        hidingObject.SetActive(false);
    }

    void Update()
    {
        if (anxietyMeter != null)
        {
            float currentFill = anxietyMeter.GetCurrentFill();

            if (currentFill >= anxietyThreshold)
            {
                Debug.Log("Object activated");
                hidingObject.SetActive(true);
            }
            else
            {
                Debug.Log("Object deactivated");
                hidingObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.activeSelf && other.CompareTag("Player"))
        {
            Debug.Log("Player entered hiding area");
            // player hiding logic here
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited hiding area");
            // player leaving hiding logic here
        }
    }
}
