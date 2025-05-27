using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    Player gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider belongs to the player and update the checkpoint position in the game controller
        if (collision.CompareTag("Player"))
        {            
            gameController.UpdateCheckpointPosition(transform.position);
        }
    }
}
