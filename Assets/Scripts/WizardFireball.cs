using UnityEngine;

public class WizardFireball : MonoBehaviour
{
    [Header("Fireball Settings")]
    public float speed = 7f;
    public float lifetime = 6f;
    public bool facingLeft;
    public Animator animator;
    public GameObject player;
    private bool hasHit = false;

    [Header("Audio")]
    public AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        audioManager.PlaySFX(audioManager.magicDrop, 0.8f);
    }

    private void Update()
    {
        if (hasHit) return;

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
        hasHit = true;

        animator.SetTrigger("Hit");

        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<Player>().TakeDamage(1);
            Debug.Log("Player hit by fireball!");
        }

        audioManager.PlaySFX(audioManager.magicExplode, 0.8f);
        Destroy(gameObject, 0.5f);
    }
}
