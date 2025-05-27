using UnityEngine;

public class WizardFireball : MonoBehaviour
{
    public float speed = 7f;
    public float lifetime = 6f;
    public bool facingLeft;
    public Animator animator;
    public GameObject player;
    private bool hasHit = false;

    private void Start()
    {
        //Destroy(this);
    }

    private void Update()
    {
        if (hasHit) return; // Si ya ha colisionado, no hacer nada más

        if (facingLeft)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
        {
            Destroy(gameObject, lifetime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hasHit = true; // Marcar como colisionado para evitar más movimientos
        //Debug.Log("Fireball collided with: " + collision.gameObject.name);

        animator.SetTrigger("Hit");

        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<Player>().TakeDamage(1);
            Debug.Log("Player hit by fireball!");
        }

        Destroy(gameObject, 0.5f);
    }
}
