using UnityEngine;

public class Wizard : MonoBehaviour
{
    private float waitedTime;
    public float waitTimeToAttack = 3;
    public Animator animator;
    public GameObject fireballPrefab;
    public Transform startPoint;
    public BoxCollider2D boxCollider2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        waitedTime += Time.fixedDeltaTime;
        if (waitedTime >= waitTimeToAttack)
        {
            Attack();
            waitedTime = 0;
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

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

    public void OnDieAnimationEnd()
    {
        // Colisionador se vuelve invisible
        boxCollider2D.size = Vector2.zero; 

        // Desactiva el Animator para que no cambie más el sprite
        animator.enabled = false;

        // Desactiva scripts de movimiento o colisión si es necesario
        this.enabled = false;
    }
}
