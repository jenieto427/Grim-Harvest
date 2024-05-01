using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float energy = 30;
    public int plantMaterial = 0;
    public int stimulant = 0;
    public float energyBound = 30;
    public int phytomass = 20; //Start with default 50 mass
    public int seenTutorial = 0; // 0 Is false
    private UIManager uiManager;
    private PlayerDataManager playerDataManager;

    void Start()
    {
        playerDataManager = GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) { Medicate(); } // Check if player ate their meds
        if (Input.GetKeyDown(KeyCode.T)) { Travel(); } // Travel between forest and village
        if (Input.GetKeyDown(KeyCode.C)) { playerDataManager.resetPlayerPrefs(); } // Check if player reset stats
    }
    public void Travel()
    {
        playerDataManager.DecreaseEnergy(0.5f);
        // Get the current scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the current scene is not 'Village'
        SceneLoader.LoadScene(currentSceneName == "Village" ? "MapGenerationTest" : "Village");
    }
    public void playerDeath()
    {
        playerDataManager.resetPlayerPrefs(); // Reset player variables

        uiManager.UpdateNotificationQueue("I'm pretty sure you died..."); // Tell the player what happened

        // Put them back in the village
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneLoader.LoadScene(currentSceneName == "Village" ? "MapGenerationTest" : "Village");

    }

    public void Medicate()
    {
        if (playerDataManager.stimulant >= 1)
        {
            playerDataManager.DecrementStimulant(); // Reduce stimulant stash
            playerDataManager.IncreaseEnergy(2); // Restore energy

            // Optionally, trigger some UI feedback or effects here
            uiManager.UpdateNotificationQueue("Brain activity has increased");
        }
        else
        {
            uiManager.UpdateNotificationQueue("You don't have any stims...");
        }
    }
}
