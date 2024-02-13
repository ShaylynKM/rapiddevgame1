using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 3f;
    public float sprintSpeedMultiplier = 2f;
    public float sprintDuration = 0.2f;
    private float sprintEndTime = -1f;

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
    private Vector2 lastMoveDirection = Vector2.right;

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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartSprint();
        }

        if (!isFrozen && GameManager.Instance.CanShoot && Input.GetKeyDown(KeyCode.Space))
        {
            HandleShooting(projectilePrefab);
        }


    }


    void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (!isFrozen)
        {
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
        }

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;


        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * moveSpeed * Time.deltaTime;


        if (moveDirection != Vector2.zero)
        {
            shootDirection = moveDirection;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
        if (moveDirection != Vector2.zero)
        {
            lastMoveDirection = moveDirection;
        }
    }

    void Move()
    {
        if (!isFrozen)
        {
            if (Time.time <= sprintEndTime)
            {
                rb.velocity = lastMoveDirection * moveSpeed * sprintSpeedMultiplier;
            }
            else
            {
                rb.velocity = moveDirection * moveSpeed;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void StartSprint()
    {
        if (!isFrozen)
        {
            sprintEndTime = Time.time + sprintDuration;
            rb.velocity = lastMoveDirection * moveSpeed * sprintSpeedMultiplier;
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

    void HandleShooting(GameObject projectilePrefab)
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