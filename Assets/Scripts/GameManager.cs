using UnityEngine;

/// <summary>
/// Manages the overall state and behavior of the game, including persistent data and global settings.
/// </summary>
/// <remarks>This class is responsible for maintaining game-wide state, such as whether the game is active and the
/// player's coin count. It persists across scene transitions by preventing its destruction when a new scene is
/// loaded.</remarks>
public class GameManager : MonoBehaviour
{
    public bool isGameActive = true;
    public bool isDialogueActive = false;
    public int coinCount = 0;

    public int level2_totalEnemies = 4;
    public int level2_enemiesKilled = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
