using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 3f;
    public float sprintSpeedMultiplier = 2f;

    public float freezeDuration = 5f;
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
    public bool isFrozen = false;
    private float freezeTimer = 0f;

    public Rigidbody2D rb;
    private Vector2 moveDirection = Vector2.zero;


    void Start()
    {
        AudioManager.Instance.Play(0, "bossFight", true);
    }

    void Update()
    {

        if (isFrozen)
        {
            freezeTimer += Time.deltaTime;

            if (freezeTimer >= freezeDuration)
            {
                UnfreezePlayer();
            }
        }
        else if (GameManager.Instance.CanMove)
        {
            HandleMovement();
        }


        ProcessInputs();

        HandleShooting(projectilePrefab);
    }


    void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (!isFrozen)
        {
            // if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) //Don't get Input.GetKey. Input.GetAxis("Horizontal), etc 
            if (Input.GetAxis("Horizontal") < -.1f)
            {
                moveX = -1f;
                spriteRenderer.sprite = leftSprite;
            }
            else if (Input.GetAxis("Horizontal") > .1f)
            // else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                moveX = 1f;
                spriteRenderer.sprite = rightSprite;
            }
            if (Input.GetAxis("Vertical") > .1f)
            // if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                moveY = 1f;
                spriteRenderer.sprite = upSprite;
            }
            else if (Input.GetAxis("Vertical") < -.1f)
            //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                moveY = -1f;
                spriteRenderer.sprite = downSprite;
            }
        }

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;


        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * moveSpeed * Time.deltaTime;


        if (moveDirection != Vector2.zero)
        {
            shootDirection = moveDirection;
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

            AudioManager.Instance.Play(1, "PlayerShoot", false);

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