using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;
    private GameObject player;
    private GameObject uiInteract;

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

        // Attempt to find the Player and UI-Interact GameObjects
        player = GameObject.Find("Player");
        uiInteract = GameObject.Find("UI-Interact");
    }

    public void TriggerMinigame(Transform playerTransform)
    {
        // Store player's position and rotation
        playerStartPosition = playerTransform.position;
        playerStartRotation = playerTransform.rotation;

        // Disable the Player and UI-Interact objects
        if (player) player.SetActive(false);
        if (uiInteract) uiInteract.SetActive(false);

        // Load a minigame additively
        SceneManager.LoadScene("SmellPlants", LoadSceneMode.Additive);
    }

    public void ReturnToMainScene()
    {
        StartCoroutine(UnloadSceneAndProceed("SmellPlants"));
    }

    private IEnumerator UnloadSceneAndProceed(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);

        // Log that the minigame scene was unloaded
        Debug.Log("Minigame scene unloaded: " + sceneName);

        // Re-enable the Player and UI-Interact objects
        if (player) player.SetActive(true);
        if (uiInteract) uiInteract.SetActive(true);

        // Optionally, set the active scene back to the main scene if needed
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MapGenerationTest"));
    }

    public void IncrementReward()
    {
        // Implement reward increment logic here
    }
}
