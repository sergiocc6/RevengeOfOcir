using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Coins")]
    public Text coinText;
    public int currentCoin = 0;

    [Header("Health")]
    public int maxHealth = 3;
    public Text health;

    [Header("Movement")]
    private float movement = 0f;
    public float moveSpeed = 7f;
    private bool facingRight = true;

    [Header("Jump")]
    public float jumpSpeed = 15f;
    public float doubleJumpSpeed = 10f;
    public bool isGround = true;
    bool canDoubleJump;

    [Header("Wall Jump & Sliding")]
    bool isTouchingFront = false;
    bool wallSliding;
    public float wallSlidingSpeed = 0.75f;
    bool isTouchingWallRight;
    bool isTouchingWallLeft;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRadious = 1.7f;
    public LayerMask attackLayer;

    public bool touchNextLevel = false;

    public Animator animator;

    [Header("UI")]
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;

    [Header("Audio")]
    public AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(maxHealth <= 0)
        {            
            Die();
        }
        coinText.text = currentCoin.ToString();
        health.text = maxHealth.ToString();

        movement = Input.GetAxis("Horizontal");

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

        // --- PASOS EN BUCLE ---
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
                canDoubleJump = true;
                isGround = false;
                animator.SetBool("Jump", true);
                audioManager.PlaySFX(audioManager.jump, 1f); // Usa la versión que permite superposición
            }
            else if (canDoubleJump && !wallSliding)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpSpeed);
                canDoubleJump = false;
                animator.SetBool("DoubleJump", true);
                audioManager.PlaySFX(audioManager.jump, 1f);
            }
            else if (wallSliding)
            {
                float direction = isTouchingWallRight ? -1 : 1;
                isTouchingWallLeft = !isTouchingWallRight;
                rb.linearVelocity = new Vector2(direction * 10f, jumpSpeed * 0.8f);
                facingRight = direction > 0;
                transform.eulerAngles = facingRight ? new Vector3(0f, 0f, 0f) : new Vector3(0f, -180f, 0f);
                wallSliding = false;
                canDoubleJump = true;
                animator.SetBool("Jump", true);
                audioManager.PlaySFX(audioManager.jump, 1f);
            }
        }

        //Caer
        if (rb.linearVelocity.y < 0f)
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
        if (Math.Abs(movement) > .1f)
        {
            //audioManager.PlaySFX(audioManager.steps);
            animator.SetFloat("Run", 1f);
        }
        else if (movement < .1f)
        {
            //audioManager.PlaySFX(audioManager.steps);
            animator.SetFloat("Run", 0f);
        }

        //Ataque (click izquierdo)
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            //audioManager.PlaySFX(audioManager.sword);
            //audioManager.PlaySFX(audioManager.sword, 1f);
        }

        //Deslizarse pared
        if (isTouchingFront && !isGround)
        {
            wallSliding = true;
            //audioManager.PlaySFX(audioManager.wallSlide);
            //audioManager.PlaySFX(audioManager.wallSlide, 1f);
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

        //Menu Pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameOverUI.activeSelf)
            {
                gameOverUI.SetActive(false);
                Time.timeScale = 1;
            }
            else if (pauseMenuUI.activeSelf)
            {
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            canDoubleJump = true;
            animator.SetBool("Jump", false);
            animator.SetBool("DoubleJump", false);
            animator.SetBool("Fall", false);
            animator.SetBool("Wall", false);
        }

        if(collision.gameObject.tag == "Next Level")
        {
            MainMenu menu = gameObject.AddComponent<MainMenu>();
            menu.ChangeScene("Demo");
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

        if(collision.gameObject.tag == "")
        {
            touchNextLevel = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isTouchingFront = false;
        isTouchingWallRight = false;
        isTouchingWallLeft = false;
    }

    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadious, attackLayer);
        if (collInfo)
        {
            if(collInfo.gameObject.GetComponent<SkeletonPatrol>() != null)
            {
                //The patrol enemy get 1 point of damage
                collInfo.gameObject.GetComponent<SkeletonPatrol>().TakeDamage(1);
            }
        }
    }

    public void PlaySwordSound()
    {
        audioManager.PlaySFX(audioManager.sword, 1f);
    }

    public void PlayWallSlideSound()
    {
        audioManager.PlaySFX(audioManager.wallSlide, 1f);
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRadious);
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            return;
        }

        maxHealth -= damage;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //TODO: cambiar para el resto de monedas que den habilidades. Por ahora sólo está para una y no hace nada
        if(other.gameObject.tag == "Coin")
        {
            //audioManager.PlaySFX(audioManager.coin);
            audioManager.PlaySFX(audioManager.coin, 1f);
            currentCoin += 1;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");            
            Destroy(other.gameObject, 1f);
        }

        if(other.gameObject.tag == "Next Level")
        {
            Debug.Log("Victory!");
            FindAnyObjectByType<SceneManagement>().LoadSecondLevel();
        }
    }

    void Die()
    {
        Debug.Log("Player died");
        audioManager.PlaySFX(audioManager.death, 1f);
        audioManager.StopMusic();
        //audioManager.PlaySFX(audioManager.death);
        //audioManager.PlayMusic(audioManager.battle);
        //audioManager.StopMusic(audioManager.battle, 0.5f);
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        FindAnyObjectByType<GameManager>().isGameActive = false;
        Destroy(this.gameObject);
    }
}
