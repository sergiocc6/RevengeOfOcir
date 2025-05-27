using UnityEngine;

public class MagickAttackRay : MonoBehaviour
{
    private bool checkHit = false;
    private bool playerHitted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider belongs to the player and Player takes damage
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
        // Check if the collider belongs to the player and Player takes damage
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

    /// <summary>
    /// Destroys the game object this script is attached to.
    /// </summary>
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
