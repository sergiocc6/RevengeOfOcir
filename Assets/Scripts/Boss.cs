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
    public float verticalDistance = 5f;
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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Guarda la escala original positiva
        originalScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);

        // Si debe empezar mirando a la izquierda, invierte la escala X
        //transform.localScale = new Vector2(-originalScale.x, originalScale.y);
        facingLeft = true;
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
            if(Math.Abs(transform.position.y - playerPosition.position.y) > verticalDistance)
            {
                inDetectRange = false; // Player is too high, do not engage
            }
            else
            {
                inDetectRange = true; // Player is within horizontal and vertical range
            }

            //inDetectRange = true;
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
            }

            // Set the boss direction based on the player's position
            float direction = playerPosition.position.x - transform.position.x;

            if (Mathf.Abs(direction) > flipMargin)
            {
                if (direction > 0 && facingLeft)
                {
                    transform.localScale = new Vector2(-originalScale.x, originalScale.y);
                    facingLeft = false;
                }
                else if (direction < 0 && !facingLeft)
                {
                    transform.localScale = new Vector2(originalScale.x, originalScale.y);
                    facingLeft = true;
                }
            }

            if (!isAttacking)
            {
                // Check if the player is within mele attack range
                if (Math.Abs(direction) <= attackRangeMele && Time.time >= lastMeleAttackTime + meleAttackCooldown)
                {
                    isAttacking = true;
                    Debug.Log("Mele Attack");
                    animator.SetBool("Run", false);
                    animator.SetTrigger("MeleAttack");
                    lastMeleAttackTime = Time.time; // save the time of the mele attack
                }
                // Check if the player is within magic attack range
                else if (Math.Abs(direction) > attackRangeMele && Math.Abs(direction) <= attackRangeMagic && Time.time >= lastMagicAttackTime + magicAttackCooldown)
                {
                    isAttacking = true;
                    Debug.Log("Magic Attack");
                    animator.SetBool("Run", false);
                    animator.SetTrigger("MagicAttack");
                    lastMagicAttackTime = Time.time; // save the time of the magic attack
                }
                // If the player is not in attack range, chase the player
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

    /// <summary>
    /// Reduces the boss's health by the specified amount of damage.
    /// </summary>
    /// <remarks>If the boss's health is already zero or less, this method has no effect.</remarks>
    /// <param name="damage">The amount of damage to apply. Must be a non-negative value.</param>
    public void TakeDamage(int damage)
    {
        Debug.Log("Boss took damage: " + damage);

        if (maxHealth <= 0)
        {
            return;
        }

        maxHealth -= damage;
    }

    /// <summary>
    /// Handles the death behavior of the entity.
    /// </summary>
    /// <remarks>This method triggers the death animation, sets the entity's state to dead,  and plays the
    /// associated death sound effect. It is intended to be called  when the entity's health reaches zero or other death
    /// conditions are met.</remarks>
    private void Die()
    {
        animator.SetTrigger("Die");
        isDead = true;
        audioManager.PlaySFX(audioManager.monsterDeath);
    }

    /// <summary>
    /// Handles the actions to be performed when the death animation ends.
    /// </summary>
    /// <remarks>This method plays a specific background music track, disables the boss game object,  and
    /// schedules its destruction after a delay of 2 seconds. It is typically called  at the conclusion of the boss's
    /// death animation.</remarks>
    public void OnDeathAnimationEnd()
    {
        // Play death sound
        audioManager.PlayMusic(audioManager.backgroundLevel2);

        // Disable the boss game object
        gameObject.SetActive(false);
        
        Destroy(gameObject, 2f); // Destroy the boss after 2 seconds
    }

    /// <summary>
    /// Executes a melee attack, dealing damage to a player within the attack radius.
    /// </summary>
    /// <remarks>The method checks for a player within the specified attack radius and applies damage if a
    /// player is detected. It also plays a sound effect associated with the melee attack.</remarks>
    public void MeleAttack()
    {
        audioManager.PlaySFX(audioManager.sword2, 1f);

        // Check if the player is within the attack radius
        Collider2D colliderInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayerMask);

        if (colliderInfo && colliderInfo.gameObject.GetComponent<Player>())
        {
            colliderInfo.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }

    /// <summary>
    /// Executes a magic attack by spawning a magic attack object at a position relative to the player.
    /// </summary>
    /// <remarks>This method plays a sound effect and instantiates a magic attack prefab at a position offset
    /// vertically  from the player's current position. The prefab is instantiated with no rotation.</remarks>
    public void MagicAttack()
    {
        audioManager.PlaySFX(audioManager.thunder, 1f);

        // Calculate the spawn position for the magic attack prefab
        Vector2 spawnPosition = new Vector2(playerPosition.position.x, playerPosition.position.y + magicAttackYOffset);

        Instantiate(magicAttackPrefab, spawnPosition, Quaternion.identity);
    }

    /// <summary>
    /// Ends the current attack sequence and resets the attack state.
    /// </summary>
    /// <remarks>This method stops the attack by setting the internal attack state to false  and ensures the
    /// associated animation transitions out of the attack state.</remarks>
    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("Run", false);
    }

    /// <summary>
    /// Draws visual debugging aids in the Scene view to represent detection and attack ranges.
    /// </summary>
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
