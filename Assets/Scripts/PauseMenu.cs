using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{    
    public GameObject pauseMenu;

    /// <summary>
    /// Pauses the game by activating the pause menu and setting the game state to inactive.
    /// </summary>
    /// <remarks>This method displays the pause menu and updates the game state to indicate that the game is
    /// no longer active.  Ensure that the game is in a state where pausing is appropriate before calling this
    /// method.</remarks>
    public void Pause()
    {
        pauseMenu.SetActive(true);
        FindAnyObjectByType<GameManager>().isGameActive = false;
    }

    /// <summary>
    /// Loads the main menu scene and sets the game state to active.
    /// </summary>
    /// <remarks>This method transitions the application to the "MainMenu" scene and updates the game state 
    /// by setting the <see cref="GameManager.isGameActive"/> property to <see langword="true"/>. Ensure that the
    /// "MainMenu" scene is properly configured in the project and that a  <see cref="GameManager"/> instance exists in
    /// the current context.</remarks>
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        FindAnyObjectByType<GameManager>().isGameActive = true;
    }

    /// <summary>
    /// Resumes the game by deactivating the pause menu and setting the game state to active.
    /// </summary>
    /// <remarks>This method is typically called to exit the paused state and resume gameplay.  It ensures the
    /// pause menu is hidden and updates the game state to indicate that the game is active.</remarks>
    public void Resume()
    {
        pauseMenu.SetActive(false);
        FindAnyObjectByType<GameManager>().isGameActive = true;
    }

    /// <summary>
    /// Restarts the current game by reloading the active scene and setting the game state to active.
    /// </summary>
    /// <remarks>This method sets the game state to active and reloads the currently active scene.  It is
    /// typically used to reset the game after a loss or to start a new session. Ensure that the <see
    /// cref="GameManager"/> object is properly initialized and accessible  before calling this method.</remarks>
    public void Restart()
    {
        FindAnyObjectByType<GameManager>().isGameActive = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
