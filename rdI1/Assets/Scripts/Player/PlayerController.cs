using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public float shootDistance = 4f;
    public SpriteRenderer spriteRenderer;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private Vector2 shootDirection = Vector2.up;

    void Start()
    {
        AudioManager.Instance.Play(0, "bossFight", true);
    }

    void Update()
    {
        
        HandleMovement();
        HandleShooting();
    }

    void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            shootDirection = Vector2.left;
            spriteRenderer.sprite = leftSprite;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            shootDirection = Vector2.right;
            spriteRenderer.sprite = rightSprite;
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            shootDirection = Vector2.up;
            spriteRenderer.sprite = upSprite;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            shootDirection = Vector2.down;
            spriteRenderer.sprite = downSprite;
        }
    }

    void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (DialogueManager.Instance.isDialogueActive)
            {
                return; 
            }

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = shootDirection * projectileSpeed;

            AudioManager.Instance.Play(1, "PlayerShoot", false);

            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Destroy(projectile, shootDistance / projectileSpeed);
        }
    }
}