using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{    
    public GameObject pauseMenu;

    
    public void Pause()
    {
        pauseMenu.SetActive(true);
        FindAnyObjectByType<GameManager>().isGameActive = false;
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        FindAnyObjectByType<GameManager>().isGameActive = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        FindAnyObjectByType<GameManager>().isGameActive = true;
    }
    public void Restart()
    {
        FindAnyObjectByType<GameManager>().isGameActive = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
