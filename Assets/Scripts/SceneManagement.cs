using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameManager gameManager;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {

            if (pauseMenuUI.activeInHierarchy)
            {
                ResumePause();
            }
            else if (settingsMenuUI.activeInHierarchy)
            {
                settingsMenuUI.SetActive(false);
                pauseMenuUI.SetActive(true);
            }
        }
    }

    public void Retry()
    {
        Debug.Log("Retry");
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
        gameManager.isGameActive = false;
        SceneManager.LoadScene("Settings");
    }

    public void LoadSecondLevel()
    {
        SceneManager.LoadScene("Level2");
    }

    public void LoadEndGame()
    {
        SceneManager.LoadScene("EndGame");
    }

    public void ResumePause()
    {
        Time.timeScale = 1;
        gameManager.isGameActive = true;
        pauseMenuUI.SetActive(false);
    }

    public void LoadSettingsPause()
    {
        gameManager.isGameActive = false;
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void LoadSettingsPause(GameObject settingMenu)
    {
        if (pauseMenuUI == null)
        {
            pauseMenuUI = GameObject.FindGameObjectWithTag("Pause UI");
        }
        if (gameManager == null)
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
        }

        gameManager.isGameActive = false;
        pauseMenuUI.SetActive(false);
        settingMenu.SetActive(true);
    }

    public void PauseMenuBack(GameObject previousMenu = null)
    {
        if (previousMenu != null)
        {
            previousMenu.SetActive(false);
        }

        gameManager.isGameActive = false;
        pauseMenuUI.SetActive(true);
    }

    public void CloseMessage(GameObject message)
    {
        if(Time.timeScale ==0)
        {
            Time.timeScale = 1;
        }
        message.SetActive(false);
    }
}
