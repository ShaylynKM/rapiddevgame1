using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneChildren : MonoBehaviour
{
    public SafeZones safeZones;
    // Start is called before the first frame update
    void Start()
    {
        safeZones = GetComponentInParent<SafeZones>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hello");
        GameObject otherObject = other.gameObject;

        if (otherObject == other.gameObject.CompareTag("Player"))
        {
            safeZones.PlayerEntered();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;

        if (other.gameObject.CompareTag("Player"))
        {
            safeZones.PlayerExited();
        }
    }
}

