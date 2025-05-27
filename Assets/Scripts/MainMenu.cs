using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenSettings()
    {
        ChangeScene("Settings");
    }

    public void OpenMenuControllers()
    {
        ChangeScene("MenuControllers");
    }

    public void OpenMenu()
    {
        ChangeScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
