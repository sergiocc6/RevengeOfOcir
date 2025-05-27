using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
    private bool isDead = false;

    [Header("Movement")]
    public bool facingLeft = true;
    public float moveSpeed = 3f;
    public Transform checkPoint;
    private Vector2 originalScale;
    private float flipMargin = 1f;

    [Header("Detect Player")]
    public bool inDetectRange = false;
    public Transform playerPosition;
    public float detectRange = 15f;
    public float attackRangeMele = 3f;
    public float attackRangeMagic = 14f;
    public float chaseSpeed = 5f;
    public Animator animator;
    bool previousPlayerState = false;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRadius = 2f;
    public LayerMask attackLayerMask;
    public int damage = 1;
    private bool isAttacking = false;
    private float meleAttackCooldown = 2f;
    private float lastMeleAttackTime = -Mathf.Infinity;

    [Header("Magic Attack")]
    public GameObject magicAttackPrefab;
    public float magicAttackYOffset = 6f;
    private float magicAttackCooldown = 3f;
    private float lastMagicAttackTime = -Mathf.Infinity;


    [Header("Audio")]
    public AudioManager audioManager;

    public GameManager gameManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale;
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
                audioManager.PlayMusic(audioManager.epicBattle);
                //TODO: Play Boss snarl sound
            }

            // Set the boss direction based on the player's position
            float direction = playerPosition.position.x - transform.position.x;

            if (Mathf.Abs(direction) > flipMargin)
            {
                if (direction > 0 && facingLeft)
                {
                    transform.localScale = new Vector2(originalScale.x, originalScale.y);
                    facingLeft = false;
                }
                else if (direction < 0 && !facingLeft)
                {
                    transform.localScale = new Vector2(-originalScale.x, originalScale.y);
                    facingLeft = true;
                }
            }

            if (!isAttacking)
            {
                if (Math.Abs(direction) <= attackRangeMele && Time.time >= lastMeleAttackTime + meleAttackCooldown)
                {
                    isAttacking = true;
                    Debug.Log("Mele Attack");
                    animator.SetBool("Run", false);
                    animator.SetTrigger("MeleAttack");
                    lastMeleAttackTime = Time.time; // Guardamos el momento del ataque cuerpo a cuerpo
                }
                else if (Math.Abs(direction) > attackRangeMele && Math.Abs(direction) <= attackRangeMagic && Time.time >= lastMagicAttackTime + magicAttackCooldown)
                {
                    isAttacking = true;
                    Debug.Log("Magic Attack");
                    animator.SetBool("Run", false);
                    animator.SetTrigger("MagicAttack");
                    lastMagicAttackTime = Time.time; // Guardamos el momento del ataque mágico
                }
                else
                {
                    Debug.Log("Chase Player");
                    // Chase the player
                    animator.SetBool("Run", true);
                    transform.position = Vector2.MoveTowards(transform.position, playerPosition.position, chaseSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            animator.SetBool("Run", false);
        }

        previousPlayerState = inDetectRange;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Boss took damage: " + damage);

        if (maxHealth <= 0)
        {
            return;
        }

        maxHealth -= damage;
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        isDead = true;
        audioManager.PlaySFX(audioManager.monsterDeath);
    }

    public void OnDeathAnimationEnd()
    {
        // Play death sound
        //audioManager.PlaySFX(audioManager.death);
        audioManager.PlayMusic(audioManager.backgroundLevel2);

        // Disable the boss game object
        gameObject.SetActive(false);
        
        Destroy(gameObject, 2f); // Destroy the boss after 2 seconds
    }

    public void MeleAttack()
    {
        audioManager.PlaySFX(audioManager.sword2, 1f);
        Collider2D colliderInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayerMask);

        if (colliderInfo && colliderInfo.gameObject.GetComponent<Player>())
        {
            colliderInfo.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }

    public void MagicAttack()
    {
        audioManager.PlaySFX(audioManager.thunder, 1f);
        Vector2 spawnPosition = new Vector2(playerPosition.position.x, playerPosition.position.y + magicAttackYOffset);

        Instantiate(magicAttackPrefab, spawnPosition, Quaternion.identity);
    }

    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("Run", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRangeMele);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRangeMagic);

        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
