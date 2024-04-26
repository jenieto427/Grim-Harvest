using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    private GameObject player;
    private GameObject uiInteract;
    private string initialSceneName; // Variable to hold the name of the initial scene

    private void Awake()
    {
        // Attempt to find the Player and UI-Interact GameObjects
        player = GameObject.Find("Player");
        uiInteract = GameObject.Find("Interact-UI");
        // Store the initial scene's name
        initialSceneName = SceneManager.GetActiveScene().name;
    }

    public void TriggerMinigame(GameObject herb)
    {
        // Disable the Player and UI-Interact objects, enable mouse and cursor
        LockObjects(true);

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

        // Optionally, set the active scene back to the initial scene if needed
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(initialSceneName));

        // Re-enable the Player and UI-Interact objects
        LockObjects(false);
    }

    private void LockObjects(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            player.SetActive(false);
            uiInteract.SetActive(false);
        }
        else if (!locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player.SetActive(true);
            uiInteract.SetActive(true);
        }
    }
}
