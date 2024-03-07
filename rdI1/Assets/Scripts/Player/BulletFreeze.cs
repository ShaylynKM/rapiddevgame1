using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFreeze : MonoBehaviour
{
    public GameObject hitEffectPrefab;
    public float freezeDuration = 2f; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            BossHealth bossHealth = FindObjectOfType<BossHealth>();
            JobEnemy enemy = collision.gameObject.GetComponent<JobEnemy>();
            if (enemy != null)
            {
                enemy.Freeze(freezeDuration); 
            }

            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, 1f);
            Destroy(gameObject); 
        }
    }
}
