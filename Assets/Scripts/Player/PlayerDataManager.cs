using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataManager : MonoBehaviour
{
    public float energy = 30;
    public int plantMaterial = 0;
    public int stimulant = 0;
    public float energyBound = 30;
    public int phytomass = 20; //Start with default 50 mass
    public int minigameSampleReward = 1; // Default minigame sample reward
    public int maxSampleRewardBound = 5;
    public float minigameEnergyCost = 5; // Default minigame energy cost
    public float minEnergyCostBound = 0.1f; //Lower bound on energy cost
    public int seenTutorial = 0; // 0 Is false
    public float mouseSensitivity = 120;
    private Player player;

    void Start()
    {
        LoadPlayerPrefs();
        // The options menu has a special instance of PlayerDataManager to save mouseSensitivity
        if (SceneManager.GetActiveScene().name != "Options")
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
    }
    //===============================================================================================
    // Player variable setters
    public void DecrementStimulant()
    {
        this.stimulant--;
        if (this.stimulant < 0) { this.energy = 0; } //Clamp stimulants low bound to 0
        SavePlayerPrefs();
    }
    public void decreasePhytomass(int decreaseAmt)
    {
        if (this.phytomass <= 0) { this.phytomass = 2; }
        this.phytomass -= decreaseAmt;
    }
    public void increasePhytomass(int increaseAmt)
    {
        if (this.phytomass > 100) { this.phytomass = 100; }
        this.phytomass += increaseAmt;
    }
    public void IncrementStimulant()
    {
        this.stimulant++;
        //if (this.stimulant > 0) { this.energy = 0; } //Upper bound on num of stimulants
        SavePlayerPrefs();
    }
    public void DecreaseEnergy(float decreaseAmt)
    {
        this.energy -= decreaseAmt;
        if (this.energy <= 0) { player.playerDeath(); } //Clamp energy low bound to 0
        SavePlayerPrefs();
    }
    public void IncreaseEnergy(float increaseAmt)
    {
        this.energy += increaseAmt;
        if (this.energy > energyBound) { this.energy = energyBound; } //Clamp energy to high 30 Hz
        SavePlayerPrefs();
    }
    public void DecreasePlantMaterial(int decreaseAmt)
    {
        this.plantMaterial -= decreaseAmt;
        if (this.plantMaterial < 0) { this.plantMaterial = 0; }
        SavePlayerPrefs();
    }
    public void IncreasePlantMaterial(int increaseAmt)
    {
        this.plantMaterial += increaseAmt;
        //Upper bound on plant material
        //if (this.plantMaterial > 100000) { this.plantMaterial = 100000; }
        SavePlayerPrefs();
    }
    //============================================================================================
    // Player modifier variables
    public void increaseMinigameSampleReward(int increaseAmt)
    {
        this.minigameSampleReward += increaseAmt;
        if (this.minigameSampleReward > this.maxSampleRewardBound)
        { this.minigameSampleReward = this.maxSampleRewardBound; }
        SavePlayerPrefs();
    }
    public void decreaseMinigameEnergyCost(float decreaseAmt)
    {
        this.minigameEnergyCost -= decreaseAmt;
        if (this.minigameEnergyCost < this.minEnergyCostBound)
        { this.minigameEnergyCost = this.minEnergyCostBound; }
        SavePlayerPrefs();
    }
    //============================================================================================
    // Player setting functions
    public void setSeenTutorialFalse()
    {
        this.seenTutorial = 0;
        SavePlayerPrefs();
    }
    public void setSeenTutorialTrue()
    {
        this.seenTutorial = 1;
        SavePlayerPrefs();

    }
    public bool boolSeenTutorial()
    {
        if (this.seenTutorial == 0) { return false; }
        else { return true; }
    }
    public void setMouseSensitivity(float changedAmount)
    {
        Debug.Log("Set mouse sensitivity: " + changedAmount.ToString());
        this.mouseSensitivity = changedAmount;
        if (this.mouseSensitivity < 100) { this.mouseSensitivity = 100; }
        else if (this.mouseSensitivity > 200) { this.mouseSensitivity = 200; }
        SavePlayerPrefs();
    }
    public void resetPlayerPrefs()
    {
        this.energy = 30;
        this.plantMaterial = 0;
        this.stimulant = 0;
        this.energyBound = 30;
        this.phytomass = 50;
        this.minigameSampleReward = 1; // Default minigame sample reward
        this.minigameEnergyCost = 5; // Default minigame energy cost
        SavePlayerPrefs();
    }
    public void resetPlayerPrefsSettings()
    {
        this.mouseSensitivity = 120f;
        this.seenTutorial = 0;
        SavePlayerPrefs();
    }
    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetFloat("Energy", energy);
        PlayerPrefs.SetInt("PlantMaterial", plantMaterial);
        PlayerPrefs.SetInt("Stimulant", stimulant);
        PlayerPrefs.SetFloat("EnergyBound", energyBound);
        PlayerPrefs.SetInt("Phytomass", phytomass);

        PlayerPrefs.SetFloat("MinigameEnergyCost", minigameEnergyCost);
        PlayerPrefs.SetInt("MinigameSampleReward", minigameSampleReward);

        PlayerPrefs.SetInt("seenTutorial", seenTutorial);
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
        PlayerPrefs.Save();  // Don't forget to call Save to write to disk

    }
    public void LoadPlayerPrefs()
    {
        energy = PlayerPrefs.GetFloat("Energy", 30);  // Default to 30 if not set
        plantMaterial = PlayerPrefs.GetInt("PlantMaterial", 0);  // Default to 0 if not set
        stimulant = PlayerPrefs.GetInt("Stimulant", 0);  // Default to 0 if not set
        energyBound = PlayerPrefs.GetFloat("EnergyBound", 30);  // Default to 30 if not set
        phytomass = PlayerPrefs.GetInt("Phytomass", 0);  // Default to 0 if not set

        minigameSampleReward = PlayerPrefs.GetInt("MinigameSampleReward", 1);
        minigameEnergyCost = PlayerPrefs.GetFloat("MinigameEnergyCost", 5);

        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 120); // Default 120f
        seenTutorial = PlayerPrefs.GetInt("seenTutorial", 0);
    }
}
