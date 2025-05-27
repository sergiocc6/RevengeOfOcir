using UnityEngine;

public class Wizard : MonoBehaviour
{
    private float waitedTime;
    public float waitTimeToAttack = 2.5f;
    public Animator animator;
    public GameObject fireballPrefab;
    public Transform startPoint;
    public BoxCollider2D boxCollider2D;

    // Horizontal detection range for the player
    public float detectionRange = 15f;

    private Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find Player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerInHorizontalRange())
        {
            if (waitedTime >= waitTimeToAttack)
            {
                Attack();
                waitedTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        waitedTime += Time.fixedDeltaTime;
    }

    /// <summary>
    /// Determines whether the player is within the horizontal detection range.
    /// </summary>
    /// <remarks>This method checks the player's position relative to this object and evaluates whether the
    /// player is  within the defined horizontal range. If the player's transform is not assigned, the method returns
    /// <see langword="false"/>.</remarks>
    /// <returns><see langword="true"/> if the player's horizontal distance from this object is less than or equal to  the
    /// specified detection range and the vertical distance is less than 5 units; otherwise, <see langword="false"/>.</returns>
    private bool IsPlayerInHorizontalRange()
    {
        if (playerTransform == null)
            return false;

        float distanceX = Mathf.Abs(transform.position.x - playerTransform.position.x);
        float distanceY = Mathf.Abs(transform.position.y - playerTransform.position.y);
        return (distanceX <= detectionRange && distanceY < 4f);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    /// <summary>
    /// Creates a fireball and initializes its properties based on the current wizard's state.
    /// </summary>
    /// <remarks>The fireball is instantiated at the wizard's starting point and oriented based on the
    /// wizard's facing direction. The fireball is also linked to the player object for further interactions.</remarks>
    public void CreateFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, startPoint.position, Quaternion.identity);
        WizardFireball fireballScript = fireball.GetComponent<WizardFireball>();
        fireballScript.facingLeft = transform.localScale.x < 0;
        fireballScript.player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Death()
    {
        animator.SetTrigger("Die");
    }

    /// <summary>
    /// Handles the end of the death animation by disabling relevant components and interactions.
    /// </summary>
    /// <remarks>This method is typically called when the death animation of the object completes.  It
    /// disables the collider, stops the animator, and deactivates the script to prevent  further movement or
    /// interaction.</remarks>
    public void OnDieAnimationEnd()
    {
        // collider is disabled to prevent further interactions
        boxCollider2D.size = Vector2.zero;

        // animator is stopped to prevent further animations
        animator.enabled = false;

        // disable the script to prevent further updates
        this.enabled = false;
    }
}
