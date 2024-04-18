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
    public static Player Instance;

    void Awake()
    {
        if (Instance == null)
        {
            // This is the first player instance, make it the Singleton
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            // A different instance of this class already exists, destroy this one
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) { Medicate(); } // Check if player ate their meds
    }
    public void Medicate()
    {
        if (stimulant >= 1)
        {
            DecrementStimulant(); // Reduce stimulant stash
            IncreaseEnergy(2); // Restore energy

            // Optionally, trigger some UI feedback or effects here
            UIManager.Instance.UpdateNotificationQueue("Brain activity has increased");
        }
        else
        {
            UIManager.Instance.UpdateNotificationQueue("You don't have any stims...");
        }
    }
    public void DecrementStimulant()
    {
        this.stimulant--;
        if (this.stimulant < 0) { this.energy = 0; } //Clamp stimulants low bound to 0
    }
    public void IncrementStimulant()
    {
        this.stimulant++;
        //if (this.stimulant > 0) { this.energy = 0; } //Upper bound on num of stimulants
    }
    public void DecreaseEnergy(float decreaseAmt)
    {
        this.energy -= decreaseAmt;
        if (this.energy < 0) { Application.Quit(); } //Clamp energy low bound to 0
    }
    public void IncreaseEnergy(float increaseAmt)
    {
        this.energy += increaseAmt;
        if (this.energy > energyBound) { this.energy = energyBound; } //Clamp energy to high 30 Hz
    }
    public void DecreasePlantMaterial(int decreaseAmt)
    {
        this.plantMaterial -= decreaseAmt;
        if (this.plantMaterial < 0) { this.plantMaterial = 0; }
    }
    public void IncreasePlantMaterial(int increaseAmt)
    {
        this.plantMaterial += increaseAmt;
        //Upper bound on plant material
        //if (this.plantMaterial > 100000) { this.plantMaterial = 100000; }
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
