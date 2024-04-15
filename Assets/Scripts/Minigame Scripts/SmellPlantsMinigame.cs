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

    void Start()
    {
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
        MinigameManager.Instance.ReturnToMainScene(); // Return to the procedural world
    }

    void LoseGame()
    {
        gameStarted = false;
        instructionsText.text = "What is this?";
        if (herbObject != null) { herbObject.SetActive(false); } // Disable plant GameObject
        //TODO: Decrease crop spawn rate
        MinigameManager.Instance.ReturnToMainScene(); // Return to the procedural world
    }

    // Herb to be deleted on game loss condition, called from Minigame Manager.cs
    public void SetHerb(GameObject herb) { herbObject = herb; }
}
