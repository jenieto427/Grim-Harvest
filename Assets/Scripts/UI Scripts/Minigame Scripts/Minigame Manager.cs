using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    public GameObject minigameUI; // Assign this in the Inspector

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartMinigame()
    {
        minigameUI.SetActive(true); // Show the minigame UI
        // Any other initialization for the minigame can be done here
    }

    public void EndMinigame()
    {
        minigameUI.SetActive(false); // Hide the minigame UI
        // Clean up the minigame, save progress, etc.
    }
}
