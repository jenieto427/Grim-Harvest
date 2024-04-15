using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }
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

    public void TriggerMinigame(GameObject herb)
    {
        // Disable the Player and UI-Interact objects
        if (player) player.SetActive(false);
        if (uiInteract) uiInteract.SetActive(false);

        // Load a minigame additively
        SceneManager.LoadScene("SmellPlants", LoadSceneMode.Additive);

        // Once the scene is loaded, pass the herb to the minigame
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            SmellPlantsMinigame minigame = FindObjectOfType<SmellPlantsMinigame>();
            if (minigame != null) { minigame.SetHerb(herb); } //if game not null, pass herb
        };
    }
    // Called from minigames' scripts
    public void ReturnToMainScene()
    {
        StartCoroutine(UnloadSceneAndProceed("SmellPlants"));
    }

    private IEnumerator UnloadSceneAndProceed(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);

        // Optionally, set the active scene back to the main scene if needed
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MapGenerationTest"));

        // Re-enable the Player and UI-Interact objects
        if (player) player.SetActive(true);
        if (uiInteract) uiInteract.SetActive(true);
    }
}
