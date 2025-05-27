using UnityEngine;
using UnityEngine.SceneManagement;

public class SkeletonPatrol : MonoBehaviour
{
    [Header("Coin")]
    public GameObject coinPrefab;

    [Header("Health")]
    public int maxHealth = 2;
    private bool isDead = false;

    [Header("Movement and distances")]
    public bool facingLeft = true;
    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float groundDistante = 1f;
    public LayerMask layerMask;

    //Detect main character
    [Header("Detect Player")]
    public bool inDetectRange = false;
    public Transform playerPosition;
    public float detectRange = 10f;
    public float attackRange = 2.5f;
    public float chaseSpeed = 4f;
    public Animator animator;
    bool previousPlayerState = false;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRadious = 1.5f;
    public LayerMask attackLayerMask;
    public int damage = 1;

    [Header("Audio")]
    public AudioManager audioManager;

    public GameManager gameManager;

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
        if (gameManager.isGameActive == false || isDead)
        {
            return;
        }

        if (maxHealth <= 0)
        {
            Die();
        }

        if (Vector2.Distance(transform.position, playerPosition.position) <= detectRange)
        {
            inDetectRange = true;
        }
        else
        {
            inDetectRange = false;
        }

        if (inDetectRange)
        {
            if (inDetectRange != previousPlayerState)
            {
                audioManager.PlayMusic(audioManager.battle);
                audioManager.PlaySFX(audioManager.skeletonSnarl, 1f);
            }

            // Set direction based on player position
            float direction = playerPosition.position.x - transform.position.x;

            // Change facing direction based on player position
            if (direction > 0 && facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if (direction < 0 && !facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }

            // Only chase if the player is within the attack range
            Vector2 rayOrigin = checkPoint.position;
            RaycastHit2D hitGround = Physics2D.Raycast(rayOrigin, Vector2.down, groundDistante, layerMask);

            bool canMove = hitGround;

            if (Mathf.Abs(direction) > attackRange && canMove)
            {
                animator.SetBool("Attack1", false);
                Vector2 target = new Vector2(playerPosition.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);
            }
            else if (Mathf.Abs(direction) <= attackRange)
            {
                animator.SetBool("Attack1", true);
            }
        }
        else
        {
            //Patroling
            if (inDetectRange != previousPlayerState)
            {
                if (SceneManager.GetActiveScene().name == "FirstScene")
                {
                    audioManager.PlayMusic(audioManager.backgroundLevel1);
                }
                else if (SceneManager.GetActiveScene().name == "Level2")
                {
                    audioManager.PlayMusic(audioManager.backgroundLevel2);
                }
            }

            //Rotates Enemy if it is going to fall
            transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

            RaycastHit2D hitGround = Physics2D.Raycast(checkPoint.position, Vector2.down, groundDistante, layerMask);

            if (hitGround == false && facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if (hitGround == false && !facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }
        }

        previousPlayerState = inDetectRange;
    }

    public void Attack()
    {
        audioManager.PlaySFX(audioManager.skeletonAttack, 1f);
        Collider2D colliderInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadious, attackLayerMask);

        if (colliderInfo)
        {
            if (colliderInfo.gameObject.GetComponent<Player>() != null)
            {
                colliderInfo.gameObject.GetComponent<Player>().TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Skeleton took damage: " + damage);
        if (maxHealth <= 0)
        {
            return;
        }

        maxHealth -= damage;
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * groundDistante);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadious);
    }

    void Die()
    {
        Debug.Log(this.transform.name + " died");
        isDead = true;

        if (SceneManager.GetActiveScene().name == "FirstScene")
        {
            audioManager.PlayMusic(audioManager.backgroundLevel1);
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            audioManager.PlayMusic(audioManager.backgroundLevel2);
        }

        audioManager.PlaySFX(audioManager.skeletonSnarl, 1f);

        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }


        Destroy(this.gameObject);
    }
}
