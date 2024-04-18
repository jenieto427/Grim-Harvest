using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SmellPlantsMinigame : MonoBehaviour
{
    public TextMeshProUGUI gameCountdownText;
    public TextMeshProUGUI instructionsText;
    public Slider progressMeter;
    private float gameDuration = 5f; //Each game lasts 5 seconds
    private bool gameStarted = false;
    private float gameTimeRemaining;
    private GameObject herbObject;
    public Player player;
    public FloraData floraData;

    void Start()
    {
        //Set the Player Object to update it's values
        GameObject playerObject = GameObject.Find("Player"); // Replace "Player" with the exact name of your player GameObject
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }
        else
        {
            Debug.LogError("Player object not found in the scene!");
        }

        gameTimeRemaining = gameDuration;
        progressMeter.maxValue = 90;
        progressMeter.value = 0;
        StartGame();
    }

    void Update()
    {
        if (gameStarted && gameTimeRemaining > 0)
        {
            UpdateGameplay();
        }
        else if (gameStarted)
        {
            LoseGame();
        }
    }

    void UpdateGameplay()
    {
        gameTimeRemaining -= Time.deltaTime;
        gameCountdownText.text = Mathf.CeilToInt(gameTimeRemaining).ToString() + " Seconds Left!";
        // Handle game input
        if (Input.GetMouseButtonDown(0))
        {
            WinGame();
        }
    }

    void StartGame()
    {
        //instructionsText.text = "Ipsum Lorem";
        gameStarted = true;
    }

    void WinGame()
    {
        gameStarted = false; // End game updates

        // Update game variables
        if (player != null)
        {
            player.IncreasePlantMaterial(5);
            player.DecreaseEnergy(1f);
        }

        //Update notifications
        UIManager.Instance.UpdateNotificationQueue("Surprised you saved this one");
        // Return back to world
        MinigameManager.Instance.ReturnToMainScene(); // Return to the procedural world
    }

    void LoseGame()
    {
        gameStarted = false; // End game updates

        // Update game variables
        if (herbObject != null) { herbObject.SetActive(false); } // Disable plant GameObject
        //TODO: Decrease crop spawn rate
        if (floraData != null) { floraData.ReduceSpawnRate(1f - .5f); }
        if (player != null) { player.DecreaseEnergy(1.5f); } // Decrease Energy

        // Update notifications
        UIManager.Instance.UpdateNotificationQueue("Killing the ecosystem one plant at a time");

        // Return back to world
        MinigameManager.Instance.ReturnToMainScene(); // Return to the procedural world
    }

    // Local herb object set from Minigame Manager.cs
    public void SetHerb(GameObject herb) { herbObject = herb; }
}
