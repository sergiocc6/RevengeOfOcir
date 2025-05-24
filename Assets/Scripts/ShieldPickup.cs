using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    public GameObject dialogPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Show the dialog panel when the player collides with the shield pickup
            Time.timeScale = 0;
            dialogPanel.SetActive(true);
        }
    }
}
