using UnityEngine;

public class SkeletonPatrol : MonoBehaviour
{
    [Header("Coin")]
    public GameObject coinPrefab;

    [Header("Health")]
    public int maxHealth = 2;

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
        if (gameManager.isGameActive == false)
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
                //audioManager.PlaySFX(audioManager.skeletonSnarl);
                audioManager.PlaySFX(audioManager.skeletonSnarl, 1f);
            }

            if (playerPosition.position.x > transform.position.x && facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if (playerPosition.position.x < transform.position.x && !facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }

            if (Vector2.Distance(transform.position, playerPosition.position) > attackRange)
            {
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position, playerPosition.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack1", true);
            }
        }
        else
        {
            //Patroling

            if (inDetectRange != previousPlayerState)
            {
                audioManager.PlayMusic(audioManager.backgroundLevel1);
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
        audioManager.PlayMusic(audioManager.backgroundLevel1);
        audioManager.PlaySFX(audioManager.skeletonSnarl, 1f);
        //animator.SetBool("Died", true);

        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }


        Destroy(this.gameObject);
    }
}
