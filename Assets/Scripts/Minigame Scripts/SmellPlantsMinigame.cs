using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems; // For UI event handling
using UnityEngine.SceneManagement;

public class SmellPlantsMinigame : MonoBehaviour, IPointerUpHandler // Interface to detect pointer release
{
    public TextMeshProUGUI gameCountdownText;
    public TextMeshProUGUI instructionsText;
    public TextMeshProUGUI sampleCountText;
    public TextMeshProUGUI aromaTargetIntervalText;
    public TextMeshProUGUI sliderMeterText;
    public Slider sliderMeter; // Player's slider
    public Image noseImage;
    public Image flowerImage;

    // Sprites
    public Sprite noseBigWhiff;
    public Sprite noseNoWhiff;
    public Sprite flowerBigWhiff;
    public Sprite flowerNoWhiff;
    private float gameDuration = 10f; // Each game lasts 5 seconds
    private bool gameStarted = false;
    private float gameTimeRemaining;
    private GameObject herbObject;
    private UIManager uiManager;
    private MinigameManager minigameManager;
    private PlayerDataManager playerDataManager;
    private float targetMin, targetMax; // Target interval for the breath meter
    private int sampleCount = 0; // Number of successful matches


    void Start()
    {
        //For handling mouse and slider onClick event
        EventTrigger trigger = sliderMeter.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => OnPointerUp((PointerEventData)data));
        trigger.triggers.Add(entry);

        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None; // Free the cursor
        Cursor.visible = true; // Make the cursor visible

        SetupGameObjects();
        SetupUI();
        StartGame();
    }

    void Update()
    {
        if (gameStarted)
        {
            gameTimeRemaining -= Time.deltaTime;
            gameCountdownText.text = Mathf.CeilToInt(gameTimeRemaining).ToString() + " Seconds Left!";
            CheckGameOver();
        }
    }

    private void SetupGameObjects()
    {
        playerDataManager = GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        minigameManager = GameObject.Find("MinigameManager").GetComponent<MinigameManager>();
    }

    private void SetupUI()
    {
        sliderMeter.maxValue = 50;
        sliderMeter.onValueChanged.AddListener(HandleSliderChange);
        sliderMeter.value = 0;

        sampleCountText.text = "";
        aromaTargetIntervalText.text = "";
        sliderMeterText.text = "aroma intake: 0";
        SetRandomTargets();
    }

    private void StartGame()
    {
        gameTimeRemaining = gameDuration;
        gameStarted = true;
        sampleCount = 0;
    }

    private void HandleSliderChange(float value)
    {
        // This function can be used if needed to react to slider changes.
        sliderMeterText.text = "aroma intake: " + Mathf.RoundToInt(value).ToString();
        if (sliderMeter.value >= targetMin && sliderMeter.value <= targetMax)
        {
            // Set images for "inside interval"
            noseImage.sprite = noseBigWhiff;
            flowerImage.sprite = flowerBigWhiff;
        }
        else
        {
            // Set images for "outside interval"
            noseImage.sprite = noseNoWhiff;
            flowerImage.sprite = flowerNoWhiff;
        }
    }

    private void SetRandomTargets()
    {
        targetMin = Random.Range(0, 47); // Maximum is 47 to ensure the interval does not exceed 50
        targetMax = targetMin + 4; // Fixed range of 4 points
        aromaTargetIntervalText.text = "aroma Interval: [" + targetMin + "," + targetMax + "]";
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (sliderMeter.value >= targetMin && sliderMeter.value <= targetMax)
        {
            sampleCount++;
            sampleCountText.text = ("Sample count: " + sampleCount);
            if (sampleCount >= playerDataManager.minigameSampleReward) { WinGame(); }
            SetRandomTargets();
        }
        else
        {
            LoseGame();
        }
        sliderMeter.value = 0; // Reset player's slider to 0
    }

    private void CheckGameOver()
    {
        if (gameTimeRemaining <= 0)
        {
            gameStarted = false;
            if (sampleCount > 0)
                WinGame();
            else
                LoseGame();
        }
    }

    private void WinGame()
    {
        gameStarted = false; // Stop game running
        // Rewards
        if (SceneManager.GetActiveScene().name == "Village")
        {
            playerDataManager.IncreasePlantMaterial(1); // Player is rewarded only 1 sample in village
            playerDataManager.DecreaseEnergy(playerDataManager.minigameEnergyCost - 0.1f);
        }
        else
        {
            playerDataManager.IncreasePlantMaterial(playerDataManager.minigameSampleReward); // Player is rewarded best in pro world
            playerDataManager.increasePhytomass(1); // Player also affects the flora generation
            playerDataManager.DecreaseEnergy(playerDataManager.minigameEnergyCost - 1.5f);
        }

        // UI update
        uiManager.UpdateNotificationQueue("Surprised you saved that one");
        minigameManager.ReturnToMainScene(); //Return to main scene
    }

    private void LoseGame()
    {
        gameStarted = false; // Stop game running

        // Punishments
        if (SceneManager.GetActiveScene().name == "MapGenerationTest")
        {
            playerDataManager.decreasePhytomass(2); // Player only affects generation in pro world
        }
        if (herbObject != null) { herbObject.SetActive(false); }
        playerDataManager.DecreaseEnergy(playerDataManager.minigameEnergyCost);

        // UI update
        uiManager.UpdateNotificationQueue("haha, I knew you'd kill it");
        minigameManager.ReturnToMainScene(); // Return to main scene
    }

    public void SetHerb(GameObject herb) { herbObject = herb; }
}
