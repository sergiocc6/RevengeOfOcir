using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameActive = true;
    public int coinCount = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
