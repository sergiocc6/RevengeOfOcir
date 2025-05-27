using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        QualitySettings.vSyncCount = 0; // Disable VSync
        Application.targetFrameRate = 240;
    }
    /// <summary>
    /// Changes the currently active scene to the specified scene.
    /// </summary>
    /// <remarks>Ensure that the specified scene is added to the build settings; otherwise, the method will
    /// fail to load the scene.</remarks>
    /// <param name="sceneName">The name of the scene to load. This must match the name of a scene included in the build settings.</param>
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Open the settings menu scene.
    /// </summary>
    public void OpenSettings()
    {
        ChangeScene("Settings");
    }

    /// <summary>
    /// Navigates to the "MenuControllers" scene.
    /// </summary>
    public void OpenMenuControllers()
    {
        ChangeScene("MenuControllers");
    }

    /// <summary>
    /// Navigates to the main menu scene.
    /// </summary>
    public void OpenMenu()
    {
        ChangeScene("MainMenu");
    }

    /// <summary>
    /// Exits the game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
