using System.Collections;
using UnityEngine;

public class DialogueTriggerZone : MonoBehaviour
{
    public GameObject startDialogueUI; // Reference to the start dialogue UI GameObject
    public GameObject uncompletedDialogueUI; // Reference to the existing enemies dialogue UI GameObject
    public GameObject finalDialogueUI; // Reference to the final dialogue UI GameObject
    public GameObject character;
    public GameManager gameManager; // Reference to the GameManager
    private bool hasStarted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasStarted)
        {
            hasStarted = true; // Prevent reactivation

            StartCoroutine(StartAnimations()); // Start the animations coroutine
        }
        else if (collision.CompareTag("Player") && hasStarted)
        {
            if(gameManager.level2_enemiesKilled < gameManager.level2_totalEnemies)
            {
                uncompletedDialogueUI.SetActive(true); // Show the existing enemies dialogue UI
            }
            else
            {
                finalDialogueUI.SetActive(true); // Show the final dialogue UI
            }
            Debug.Log("Enemigos eliminados " + gameManager.level2_enemiesKilled + " de " + gameManager.level2_totalEnemies);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            WiseCatController wiseCatController = character.GetComponent<WiseCatController>();
            if(wiseCatController != null)
            {
                wiseCatController.Sat(); // Set to iddle animation when player exits
            }

            startDialogueUI.SetActive(false); // Hide the dialogue UI
        }
    }

    private IEnumerator StartAnimations()
    {
        if (character != null)
        {
            WiseCatController wiseCatController = character.GetComponent<WiseCatController>();
            if (wiseCatController != null)
            {
                wiseCatController.Sleeping(); // Start with the sleeping animation
                yield return new WaitForSeconds(wiseCatController.timeSleeping); // Wait for the sleeping animation duration
                wiseCatController.WakingUp();
                yield return new WaitForSeconds(1f); // Wait for the waking up animation to finish
                wiseCatController.Stretching();
                yield return new WaitForSeconds(1.2f); // Wait for the stretching animation to finish
                wiseCatController.Licking(); // Set to licking after stretching
                yield return new WaitForSeconds(1f);
                wiseCatController.Iddle(); // Set to iddle after licking
                startDialogueUI.SetActive(true); // Show the dialogue UI
            }
            else
            {
                Debug.LogWarning("WiseCatController component not found on character.");
            }
        }
        else
        {
            Debug.LogWarning("Character GameObject is not assigned.");
        }
    }
}
