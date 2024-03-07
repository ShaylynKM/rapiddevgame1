using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float moveSpeed = 5f;
    //public float sprintSpeedMultiplier = 2f;

    
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


    // Shaylyn's additions
    private float fireRate = 0.2f; // How fast the player can shoot
    private float timeSinceFire = 0f; // Keeps track of when the player last fired

    public PlayerHealth playerHealth;

    public Animation anim;

    void Start()
    {
        //animator.SetBool("IsWalking", false); // Sets the animator value for IsWalking to false when the player is not moving

        anim = GetComponent<Animation>(); // Finds the animation component


        //AudioManager.Instance.Play(0, "bossFight", true);

        timeSinceFire = Time.time + fireRate; // Changes rate of fire to 5 shots per second

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

        //animator.SetFloat("X", moveX); // Sets the animator value for X
        //animator.SetFloat("Y", moveY); // Sets the animator value for y
        
        if (moveDirection != Vector2.zero)
        {
            UpdateSpriteDirection(moveDirection);
            shootDirection = moveDirection;
        }

        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    rb.velocity = moveDirection * moveSpeed * sprintSpeedMultiplier;
        //}
        //else
        //{
        //    rb.velocity = moveDirection * moveSpeed;
        //}
    }



    void ProcessInputs()
    {
        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            return;
            //animator.SetBool("IsWalking", false); // Sets the animator value for IsWalking to false when the player is not moving

        }

        //animator.SetBool("IsWalking", true); // Sets the animator value for IsWalking to true when the player is moving
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        UpdateSpriteDirection(moveDirection);
        rb.velocity = moveDirection * moveSpeed;


        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    rb.velocity = moveDirection * moveSpeed * sprintSpeedMultiplier;
        //}
        //else
        //{
        //    rb.velocity = moveDirection * moveSpeed;
        //}

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

        if (Input.GetMouseButton(0) && !isFrozen && Time.time > timeSinceFire) // Changed to holding LMB; added a restriction to rate of fire
        {
            //animator.SetBool("IsThrowing", true); // Sets the animator value for throwing
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
            rbProjectile.velocity = shootDirection * projectileSpeed;

            AudioManager.Instance.Play(2, "PlayerShoot", false);

            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            timeSinceFire = Time.time + fireRate; // Resets the fire rate timer

            Destroy(projectile, shootDistance / projectileSpeed);
           //animator.SetBool("IsThrowing", false); // Resets the bool
        }
    }

    public void FreezePlayer()
    {
        isFrozen = true;
    }

    public void UnfreezePlayer()
    {
        isFrozen = false;
        
    }

}