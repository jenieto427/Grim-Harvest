using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
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
        // Travel between forest and village
        if (Input.GetKeyDown(KeyCode.T) && SceneManager.GetActiveScene().name != "StudyDungeon")
        { Travel(); }
        // Check if player reset their stats
        if (Input.GetKeyDown(KeyCode.C)) { playerDataManager.resetPlayerPrefs(); }
        // Check if the player reset their settings
        if (Input.GetKeyDown(KeyCode.Alpha1)) { playerDataManager.resetPlayerPrefsSettings(); }
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
        uiManager.UpdateNotificationQueue("I think you're dying..."); // Tell the player what happened
        playerDataManager.resetPlayerPrefs(); // Reset player variables

        // Put them back in the village
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneLoader.LoadScene(currentSceneName == "Village" ? "MapGenerationTest" : "Village");

    }

    public void Medicate()
    {
        if (playerDataManager.stimulant >= 1)
        {
            playerDataManager.DecrementStimulant(); // Reduce stimulant stash
            playerDataManager.IncreaseEnergy(3); // Restore energy

            // Optionally, trigger some UI feedback or effects here
            uiManager.UpdateNotificationQueue("Brain activity has increased");
        }
        else
        {
            uiManager.UpdateNotificationQueue("You don't have any stims...");
        }
    }
}
