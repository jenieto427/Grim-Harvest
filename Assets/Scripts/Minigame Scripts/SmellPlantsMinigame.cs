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
    public Slider progressMeter;
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
        progressMeter.maxValue = 20; // Setting the maximum value programmatically
        progressMeter.value = 0; // Ensure the progress starts at 0
    }

    void Update()
    {
        if (instructionsCountdown > 0)
        {
            instructionsCountdown -= Time.deltaTime;
            instructionsCountdownText.text = "Starting in " + Mathf.CeilToInt(instructionsCountdown).ToString() + " Seconds!";
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
                gameCountdownText.text = Mathf.CeilToInt(gameTimeRemaining).ToString() + " seconds left!";
                progressMeter.value -= fillAmountReduction * Time.deltaTime;
                if (Input.GetMouseButtonDown(0)) // Detect clicks to fill the meter
                {
                    progressMeter.value += fillAmountPerClick;
                    if (progressMeter.value >= progressMeter.maxValue)
                    {
                        WinGame();
                    }
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
        Debug.Log("Nice!");
        // Optionally, load another scene or return to the main game
    }

    void LoseGame()
    {
        // Handle lose condition (e.g., display lose message)
        Debug.Log("You killed it :(");
        // Optionally, restart the game or return to the main game
    }
}
