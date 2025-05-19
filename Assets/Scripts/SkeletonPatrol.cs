using UnityEngine;

public class SkeletonPatrol : MonoBehaviour
{
    public int maxHealth = 5;

    public bool facingLeft = true;
    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float groundDistante = 1f;
    public LayerMask layerMask;

    //Detect main character
    public bool inDetectRange = false;
    public Transform playerPosition;
    public float detectRange = 10f;
    public float attackRange = 2.5f;
    public float chaseSpeed = 4f;
    public Animator animator;

    //Attack variables
    public Transform attackPoint;
    public float attackRadious = 1.5f;
    public LayerMask attackLayerMask;
    public int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FindAnyObjectByType<GameManager>().isGameActive == false)
        {
            return;
        }

        if(maxHealth <= 0)
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
            if(playerPosition.position.x > transform.position.x && facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if(playerPosition.position.x < transform.position.x && !facingLeft)
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
    }

    public void Attack()
    {
        Collider2D colliderInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadious, attackLayerMask);

        if (colliderInfo)
        {
            if(colliderInfo.gameObject.GetComponent<Player>() != null)
            {
                colliderInfo.gameObject.GetComponent<Player>().TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if(maxHealth <= 0)
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
        //se puede poner un segundo parámetro se establece el tiempo que tarda en eliminar el objeto
        //sino se destruye instantaneamente
        Destroy(this.gameObject); ;
    }
}
