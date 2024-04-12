using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SmellPlantsMinigame : MonoBehaviour
{
    public GameObject instructionsPanel;
    public TextMeshProUGUI instructionsCountdownText;
    public GameObject gamePanel;
    public TextMeshProUGUI gameCountdownText;
    public TextMeshProUGUI instructionsText;
    public TextMeshProUGUI buttonsInstructions;
    public Slider progressMeter;
    private string minigameSceneName = "SmellPlants";
    public float gameDuration = 10f; // Game duration in seconds
    public float fillAmountPerClick = 1f; // How much the meter fills with each click
    public float fillAmountReduction = 3f; // How much the meter emptys aside of click

    private float instructionsCountdown = 3f; // Duration of the instructions countdown
    private bool gameStarted = false;
    private float gameTimeRemaining;

    void Start()
    {
        instructionsPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameTimeRemaining = gameDuration;
        progressMeter.maxValue = 90; // Setting the maximum value programmatically
        progressMeter.value = 0; // Ensure the progress starts at 0
    }

    void Update()
    {
        if (instructionsCountdown > 0)
        {
            instructionsCountdown -= Time.deltaTime;
            instructionsCountdownText.text = "Starting in " + Mathf.CeilToInt(instructionsCountdown).ToString() + "!";
        }
        else if (!gameStarted)
        {
            StartGame();
        }
        else
        {
            if (gameTimeRemaining > 0)
            {
                gameTimeRemaining -= Time.deltaTime;
                gameCountdownText.text = Mathf.CeilToInt(gameTimeRemaining).ToString() + " Seconds Left!";
                progressMeter.value -= fillAmountReduction;
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) 
                || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    progressMeter.value += fillAmountPerClick;
                    if (progressMeter.value >= progressMeter.maxValue)
                    {
                        WinGame();
                    }
                    else if(progressMeter.value < 0) {progressMeter.value = 0;}
                }
            }
            else
            {
                LoseGame();
            }
        }
    }

    void StartGame()
    {
        instructionsPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameStarted = true;
    }

    void WinGame()
    {
        // Handle win condition (e.g., display win message and reward)
        gamePanel.SetActive(false);
        instructionsPanel.SetActive(true);
        instructionsCountdownText.enabled = false;
        buttonsInstructions.enabled = false;
        instructionsText.text = "I can harvest this!";
        // Optionally, load another scene or return to the main game
        //MinigameManager.Instance.ReturnToMainScene();
    }

    void LoseGame()
    {
        // Handle lose condition (e.g., display lose message)
        gamePanel.SetActive(false);
        instructionsPanel.SetActive(true);
        instructionsCountdownText.enabled = false;
        buttonsInstructions.enabled = false;
        instructionsText.text = "What is this?";
        // Optionally, restart the game or return to the main game
        //MinigameManager.Instance.ReturnToMainScene();
    }
}
