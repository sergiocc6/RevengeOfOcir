using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSettings()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Settings");
    }

    public void LoadSecondLevel()
    {
        SceneManager.LoadScene("Level2");
    }    

    public void ResumePause()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
    }
}
