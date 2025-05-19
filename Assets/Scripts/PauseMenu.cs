using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{    
    public GameObject pauseMenu;
    bool gamePaused = false;

    public void Pause()
    {
        pauseMenu.SetActive(true);
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }
    public void Restar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
