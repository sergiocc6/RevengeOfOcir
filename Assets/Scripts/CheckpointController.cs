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
        if (collision.CompareTag("Player"))
        {
            gameController.UpdateCheckpointPosition(transform.position);
        }
    }
}
