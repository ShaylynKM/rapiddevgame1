using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public GameObject projectilePrefab;
    public GameObject freezeProjectilePrefab;
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
        if (DialogueManager.Instance.isDialogueActive)
        {
            return; // Stop all actions if dialogue is active
        }

        if (GameManager.Instance.CanMove)
        {
            HandleMovement();
        }

        if (GameManager.Instance.CanShoot && Input.GetKeyDown(KeyCode.Space))
        {
            HandleShooting(projectilePrefab);
        }

        if (GameManager.Instance.CanFreeze && Input.GetKeyDown(KeyCode.F))
        {
            HandleFreezing(freezeProjectilePrefab);

        }
    }


    void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
            spriteRenderer.sprite = leftSprite;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f;
            spriteRenderer.sprite = rightSprite;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveY = 1f;
            spriteRenderer.sprite = upSprite;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
            spriteRenderer.sprite = downSprite;
        }

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

        
        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * moveSpeed * Time.deltaTime;

       
        if (moveDirection != Vector2.zero)
        {
            shootDirection = moveDirection;
        }
    }


    void HandleFreezing(GameObject freezeProjectilePrefab)
    {

        
        if (Input.GetKeyDown(KeyCode.F))
        {

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = shootDirection * projectileSpeed;

            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Destroy(projectile, shootDistance / projectileSpeed);
        }
    }

    

    void HandleShooting(GameObject projectilePrefab)
    {
       
        if (Input.GetKeyDown(KeyCode.Space) )
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