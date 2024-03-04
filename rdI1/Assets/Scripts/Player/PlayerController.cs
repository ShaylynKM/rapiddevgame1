using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float moveSpeed = 5f;
    public float sprintSpeedMultiplier = 2f;

    
    public GameObject projectilePrefab;
   
    public float projectileSpeed = 10.0f;
    public float shootDistance = 4f;

    public SpriteRenderer spriteRenderer;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private Vector2 shootDirection = Vector2.up;
    public bool isFrozen = false;
  
    public Rigidbody2D rb;
    private Vector2 moveDirection = Vector2.zero;


    void Start()
    {
        //AudioManager.Instance.Play(0, "bossFight", true);
    }

    void Update()
    {

        if (!isFrozen && GameManager.Instance.CanMove)
        {
            HandleMovement();
        }


        ProcessInputs();

        HandleShooting(projectilePrefab);
    }


    void HandleMovement()
    {
       
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        
        if (moveDirection != Vector2.zero)
        {
            UpdateSpriteDirection(moveDirection);
            shootDirection = moveDirection;
        }

        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity = moveDirection * moveSpeed * sprintSpeedMultiplier;
        }
        else
        {
            rb.velocity = moveDirection * moveSpeed;
        }
    }



    void ProcessInputs()
    {
        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        UpdateSpriteDirection(moveDirection);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity = moveDirection * moveSpeed * sprintSpeedMultiplier;
        }
        else
        {
            rb.velocity = moveDirection * moveSpeed;
        }

        if (moveDirection != Vector2.zero)
        {
            UpdateSpriteDirection(moveDirection);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = (mousePosition - transform.position).normalized;
            shootDirection = GetClosestCardinalDirection(directionToMouse);
            UpdateSpriteDirection(shootDirection);
        }
    }

    Vector2 GetClosestCardinalDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return direction.y > 0 ? Vector2.up : Vector2.down;
        }
    }

    void UpdateSpriteDirection(Vector2 direction)
    {
        if (direction == Vector2.left)
        {
            spriteRenderer.sprite = leftSprite;
        }
        else if (direction == Vector2.right)
        {
            spriteRenderer.sprite = rightSprite;
        }
        else if (direction == Vector2.up)
        {
            spriteRenderer.sprite = upSprite;
        }
        else if (direction == Vector2.down)
        {
            spriteRenderer.sprite = downSprite;
        }
    }



    void HandleShooting(GameObject projectilePrefab)
    {

        if (isFrozen|| !GameManager.Instance.CanShoot)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !isFrozen)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
            rbProjectile.velocity = shootDirection * projectileSpeed;

            AudioManager.Instance.Play(2, "PlayerShoot", false);

            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Destroy(projectile, shootDistance / projectileSpeed);
        }
    }

    public void FreezePlayer()
    {
        if(GameManager.Instance.CanFreeze)
        {
            isFrozen = true;
        }
    }

    public void UnfreezePlayer()
    {
        if (GameManager.Instance.CanFreeze)
        {
            isFrozen = false;
        }
    }

}