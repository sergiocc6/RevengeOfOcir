using UnityEngine;

public class MagickAttackRay : MonoBehaviour
{
    private bool checkHit = false;
    private bool playerHitted = false;
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (checkHit && collision.gameObject.CompareTag("Player") && !playerHitted)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1);
                playerHitted = true; // Ensure we only hit the player once
                Debug.Log("Player hit by magick attack ray!");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (checkHit && collision.gameObject.CompareTag("Player") && !playerHitted)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1);
                playerHitted = true; // Ensure we only hit the player once
                Debug.Log("Player hit by magick attack ray!");
            }
        }
    }

    public void checkHitPlayer()
    {
        checkHit = true;
    }

    public void dontCheckHitPlayer()
    {
        checkHit = false;
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
