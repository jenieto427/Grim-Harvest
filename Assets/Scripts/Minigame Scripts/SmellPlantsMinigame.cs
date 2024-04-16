using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SmellPlantsMinigame : MonoBehaviour
{
    public TextMeshProUGUI gameCountdownText;
    public TextMeshProUGUI instructionsText;
    public Slider progressMeter;
    private float gameDuration = 10f;
    private bool gameStarted = false;
    private float gameTimeRemaining;
    private GameObject herbObject;
    public Player player;

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
        instructionsText.text = "Ipsum Lorem";
        gameStarted = true;
    }

    void WinGame()
    {
        gameStarted = false;
        instructionsText.text = "I can harvest this!";
        //TODO: Increase resources
        if (player != null)
        {
            player.IncreasePlantMaterial(5);
            player.DecreaseEnergy(1f);
            Debug.Log("Beta Activity" + player.energy);
            Debug.Log("Plant Material" + player.plantMaterial);
        }
        else { Debug.Log("Player Null"); }
        MinigameManager.Instance.ReturnToMainScene(); // Return to the procedural world
    }

    void LoseGame()
    {
        gameStarted = false;
        instructionsText.text = "What is this?";
        if (herbObject != null) { herbObject.SetActive(false); } // Disable plant GameObject
        //TODO: Decrease crop spawn rate
        //TODO: Decrease energy
        if (player != null) { player.DecreaseEnergy(1.5f); }
        MinigameManager.Instance.ReturnToMainScene(); // Return to the procedural world
    }

    // Herb to be deleted on game loss condition, called from Minigame Manager.cs
    public void SetHerb(GameObject herb) { herbObject = herb; }
}
