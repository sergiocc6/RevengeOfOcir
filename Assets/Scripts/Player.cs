using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("--- Coins ---")]
    public Text coinText;
    public int currentCoin = 0;

    [Header("--- Health ---")]
    public int maxHealth = 3;
    public Text health;
    public GameObject heartPrefab;
    public Transform heartsContainer;

    [Header("--- Movement ---")]
    private float movement = 0f;
    public float moveSpeed = 7f;
    private bool facingRight = true;

    [Header("--- Jump  ---")]
    public float jumpSpeed = 15f;
    public float doubleJumpSpeed = 10f;
    public bool isGround = true;
    bool canDoubleJump;

    [Header("--- Wall Jump & Sliding ---")]
    bool isTouchingFront = false;
    bool wallSliding;
    public float wallSlidingSpeed = 0.75f;
    bool isTouchingWallRight;
    bool isTouchingWallLeft;
    bool isWallJumping = false;

    [Header("--- Attack ---")]
    public Transform attackPoint;
    public float attackRadious = 1.7f;
    public LayerMask attackLayer;
    public int attackDamage = 1;

    [Header("--- Shield ---")]
    public bool hasShield = false;
    public bool isShieldActive = false;
    public float shieldDuration = 5f;


    bool touchNextLevel = false;

    [Header("--- Animator ---")]
    public Animator animator;

    [Header("--- UI ---")]
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;

    [Header("--- Audio ---")]
    public AudioManager audioManager;

    [Header("--- Game Management ---")]
    public GameManager gameManager;

    [Header("--- Chekpoint ---")]
    Vector2 startPosition;
    Vector2 checkpointPosition;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checkpointPosition = transform.position;
        startPosition = transform.position;
        gameManager.isGameActive = true;
        currentCoin = gameManager.coinCount;
        UpdateHearts();
    }

    // Update is called once per frame
    void Update()
    {
        //Menu Pausa y Settings
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (gameOverUI.activeSelf)
            {
                gameOverUI.SetActive(false);
                gameManager.isGameActive = true;
                Time.timeScale = 1;
            }
            else if (pauseMenuUI.activeSelf)
            {
                pauseMenuUI.SetActive(false);
                gameManager.isGameActive = true;
                Time.timeScale = 1;
            }
            else
            {
                pauseMenuUI.SetActive(true);
                gameManager.isGameActive = false;
                Time.timeScale = 0;
            }
        }

        if (gameManager.isGameActive == false)
        {
            return;
        }

        if (maxHealth <= 0)
        {
            UpdateHearts();
            Die();
        }
        coinText.text = currentCoin.ToString();
        health.text = maxHealth.ToString();

        //movement = Input.GetAxis("Horizontal");
        movement = isShieldActive ? 0f : Input.GetAxis("Horizontal");

        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && !facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        // bucle para el sonido de los pasos
        if (Math.Abs(movement) > 0.1f && isGround)
        {
            audioManager.PlayStepsLoop();
            animator.SetFloat("Run", 1f);
        }
        else
        {
            audioManager.StopStepsLoop();
            animator.SetFloat("Run", 0f);
        }

        //Salto
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (isGround)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
                canDoubleJump = true;
                isGround = false;
                animator.SetBool("Jump", true);
                audioManager.PlaySFX(audioManager.jump, 1f); // Usa la versión que permite superposición
            }
            else if (wallSliding)
            {
                float direction = isTouchingWallRight ? -1 : 1;
                isWallJumping = true;
                isTouchingWallLeft = !isTouchingWallRight;
                rb.linearVelocity = new Vector2(direction * 10f, jumpSpeed * 0.8f);
                facingRight = direction > 0;
                transform.eulerAngles = facingRight ? new Vector3(0f, 0f, 0f) : new Vector3(0f, -180f, 0f);
                wallSliding = false;
                canDoubleJump = true;
                animator.SetBool("Jump", true);
                audioManager.PlaySFX(audioManager.jump, 1f);
            }
            else if (canDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpSpeed);
                canDoubleJump = false;
                animator.SetBool("DoubleJump", true);
                audioManager.PlaySFX(audioManager.jump, 1f);
            }
        }

        //Escudo (Click derecho)
        if ((Input.GetMouseButton(1) || Input.GetKey(KeyCode.Joystick1Button3)) && hasShield)
        {
            isShieldActive = true;
            animator.SetBool("Shield", true);
        }
        else
        {
            isShieldActive = false;
            animator.SetBool("Shield", false);
        }

        //Caer
        if (rb.linearVelocity.y < 0f && !isGround)
        {
            animator.SetBool("Jump", false);
            if (!wallSliding)
                animator.SetBool("Fall", true);
        }
        else if (rb.linearVelocity.y > 0f)
        {
            animator.SetBool("Fall", false);
        }

        //Correr
        if (Math.Abs(movement) > .1f && !wallSliding)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (movement < .1f)
        {
            animator.SetFloat("Run", 0f);
        }

        //Ataque (click izquierdo)
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            animator.SetTrigger("Attack");
        }

        //Deslizarse pared
        if (isTouchingFront && !isGround)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if (wallSliding)
        {
            animator.SetBool("Wall", true);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            animator.SetBool("Wall", false);
        }
    }

    private void FixedUpdate()
    {
        if (isWallJumping)
        {
            transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
        }
        //transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
        else
        {
            if (wallSliding)
            {
                bool pressingTowardsWall = (isTouchingWallRight && movement > 0) || (isTouchingWallLeft && movement < 0);

                if (pressingTowardsWall)
                {
                    rb.linearVelocity = new Vector2(0f, -wallSlidingSpeed);
                }
                else
                {
                    rb.linearVelocity = new Vector2(movement * moveSpeed, -wallSlidingSpeed);
                }
            }
            else
                rb.linearVelocity = new Vector2(movement * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isWallJumping = false;
        Debug.Log("Collision with: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            canDoubleJump = true;
            animator.SetBool("Jump", false);
            animator.SetBool("DoubleJump", false);
            animator.SetBool("Wall", false);
            animator.SetBool("Fall", false);
        }

        if (collision.gameObject.tag == "Next Level")
        {
            SceneManagement sceneManager = FindAnyObjectByType<SceneManagement>();
            sceneManager.LoadSecondLevel();
        }

        if (collision.gameObject.tag == "RightWall")
        {
            isTouchingFront = true;
            isTouchingWallRight = true;
            animator.SetBool("Fall", false);
        }

        if (collision.gameObject.tag == "LeftWall")
        {
            isTouchingFront = true;
            isTouchingWallLeft = true;
            animator.SetBool("Fall", false);
        }

        if(collision.gameObject.tag == "Trap")
        {
            TouchTrap();
        }

        if(collision.gameObject.tag == "Water")
        {
            Die();
        }

        if (collision.gameObject.tag == "CheckPoint")
        {
            UpdateCheckpointPosition(collision.transform.position);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "RightWall")
        {
            isTouchingFront = true;
            isTouchingWallRight = true;
            animator.SetBool("Fall", false);
        }

        if (collision.gameObject.tag == "LeftWall")
        {
            isTouchingFront = true;
            isTouchingWallLeft = true;
            animator.SetBool("Fall", false);
        }

        if (collision.gameObject.tag == "")
        {
            touchNextLevel = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isTouchingFront = false;
        isTouchingWallRight = false;
        isTouchingWallLeft = false;

        if(collision.gameObject.tag == "Ground")
        {
            isGround = false;
        }
    }

    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadious, attackLayer);
        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<SkeletonPatrol>() != null)
            {
                //The patrol enemy get 1 point of damage
                collInfo.gameObject.GetComponent<SkeletonPatrol>().TakeDamage(attackDamage);
            }
        }
    }

    public void PlaySwordSound()
    {
        audioManager.PlaySFX(audioManager.sword, 1f);
    }

    public void PlayWallSlideSound()
    {
        //audioManager.PlaySFX(audioManager.wallSlide, 1f);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRadious);
    }

    public void TakeDamage(int damage, bool fromTrap = false)
    {
        if(isShieldActive && !fromTrap)
        {
            isShieldActive = false;
            animator.SetBool("Shield", false);
            return;
        }

        if (maxHealth <= 0)
        {
            return;
        }

        maxHealth -= damage;
        UpdateHearts();

        if (maxHealth > 0)
            animator.SetTrigger("TakeDamage");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //TODO: cambiar para el resto de monedas que den habilidades. Por ahora sólo está para una y no hace nada
        if (other.gameObject.tag == "Coin")
        {
            //audioManager.PlaySFX(audioManager.coin);
            audioManager.PlaySFX(audioManager.coin, 1f);
            currentCoin += 1;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            Destroy(other.gameObject, 1f);
        }

        if(other.gameObject.name == "Shield")
        {
            hasShield = true;
            other.gameObject.GetComponentInChildren<Animator>().SetTrigger("Collected");
            Destroy(other.gameObject, 0.5f);
        }

        if (other.gameObject.tag == "Next Level")
        {
            Debug.Log("Victory!");
            FindAnyObjectByType<SceneManagement>().LoadSecondLevel();
        }
    }

    void Die()
    {
        animator.SetBool("Fall", false);
        Debug.Log("Player died");
        audioManager.PlayMusic(audioManager.death);
        audioManager.StopMusic();
        FindAnyObjectByType<GameManager>().isGameActive = false;
        animator.SetTrigger("Die");
    }


    public void OnDieAnimationEnd()
    {
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }


    public void UpdateCheckpointPosition(Vector2 position)
    {
        checkpointPosition = position;
    }

    public void TouchTrap()
    {
        TakeDamage(1, true);

        if (maxHealth <= 0)
        {
            Die();
        }
        else
        {
            transform.position = checkpointPosition;
        }
        //transform.position = checkpointPosition;
    }

    public void UpdateHearts()
    {
        // Limpia los corazones antiguos
        foreach (Transform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }

        // Instancia tantos corazones como maxHealth
        for (int i = 0; i < maxHealth; i++)
        {
            Instantiate(heartPrefab, heartsContainer);
        }
    }

}