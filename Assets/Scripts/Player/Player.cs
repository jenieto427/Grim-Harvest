using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float energy = 30;
    public int plantMaterial = 0;
    public int stimulant = 0;
    public float energyBound = 30;
    public int phytomass = 0;
    public UIManager uiManager;

    void Start()
    {
        LoadPlayerPrefs();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) { Medicate(); } // Check if player ate their meds
        if (Input.GetKeyDown(KeyCode.T)) { Travel(); }
    }
    public void Travel()
    {
        DecreaseEnergy(0.5f);
        // Get the current scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the current scene is not 'Village'
        SceneLoader.LoadScene(currentSceneName == "Village" ? "MapGenerationTest" : "Village");
    }
    public void Medicate()
    {
        if (stimulant >= 1)
        {
            DecrementStimulant(); // Reduce stimulant stash
            IncreaseEnergy(2); // Restore energy

            // Optionally, trigger some UI feedback or effects here
            uiManager.UpdateNotificationQueue("Brain activity has increased");
        }
        else
        {
            uiManager.UpdateNotificationQueue("You don't have any stims...");
        }
    }
    public void DecrementStimulant()
    {
        this.stimulant--;
        if (this.stimulant < 0) { this.energy = 0; } //Clamp stimulants low bound to 0
        SavePlayerPrefs();
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
        if (this.energy < 0) { Application.Quit(); } //Clamp energy low bound to 0
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

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetFloat("Energy", energy);
        PlayerPrefs.SetInt("PlantMaterial", plantMaterial);
        PlayerPrefs.SetInt("Stimulant", stimulant);
        PlayerPrefs.SetFloat("EnergyBound", energyBound);
        PlayerPrefs.SetInt("Phytomass", phytomass);

        PlayerPrefs.Save();  // Don't forget to call Save to write to disk

        //uiManager.UpdateInteractionUI("Saved player stats");
    }

    public void LoadPlayerPrefs()
    {
        energy = PlayerPrefs.GetFloat("Energy", 30);  // Default to 30 if not set
        plantMaterial = PlayerPrefs.GetInt("PlantMaterial", 0);  // Default to 0 if not set
        stimulant = PlayerPrefs.GetInt("Stimulant", 0);  // Default to 0 if not set
        energyBound = PlayerPrefs.GetFloat("EnergyBound", 30);  // Default to 30 if not set
        phytomass = PlayerPrefs.GetInt("Phytomass", 0);  // Default to 0 if not set

        //uiManager.UpdateInteractionUI("Loaded player stats");
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        energy = data.energy;
        plantMaterial = data.plantMaterial;
        stimulant = data.stimulant;

        //Check if we are in the procedural world to load the player's saved position
        if ("MapGenerationTest" != SceneManager.GetActiveScene().name) { return; }
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
    }
}
