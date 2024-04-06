using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    private void Awake()
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

    public void TriggerMinigame(Transform playerTransform)
    {
        // Store player's position and rotation
        playerStartPosition = playerTransform.position;
        playerStartRotation = playerTransform.rotation;

        // Load a random minigame
        SceneManager.LoadScene("SmellPlants"); // Replace with random selection logic if more scenes
    }

    public void ReturnToMainScene()
    {
        // Load the main scene (replace "MainScene" with your actual scene name)
        SceneManager.LoadScene("MapGenerationTest", LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnMainSceneLoaded;
    }

    private void OnMainSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Set the player's position and rotation to the stored values
            player.transform.position = playerStartPosition;
            player.transform.rotation = playerStartRotation;
        }

        // Unsubscribe to prevent this method from being called on every scene load
        SceneManager.sceneLoaded -= OnMainSceneLoaded;
    }

    public void IncrementReward()
    {
        // Logic to increment rewards or variables
    }
}
