using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour
{
    private UIManager uiManager;
    private PlayerDataManager playerDataManager;
    private NPCWizardAnimationController animationController;


    private void Awake()
    {
        playerDataManager = GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>();
        GameObject uiManagerObject = GameObject.Find("UIManager");
        uiManager = uiManagerObject.GetComponent<UIManager>();
        GameObject npcObject = GameObject.Find("Wizard"); // Replace "NPC" with your NPC's name in the scene
        if (SceneManager.GetActiveScene().name == "Village")
        {
            animationController = npcObject.GetComponent<NPCWizardAnimationController>();
        }

    }

    public void HandleVendorInteraction()
    {
        int cost = 5; // Cost in plant materials for one stimulant
        if (playerDataManager.plantMaterial >= cost)
        {
            playerDataManager.DecreasePlantMaterial(cost); // Reduce samples/money
            playerDataManager.IncrementStimulant(); // Increment stimulant count 
            // Show success messages
            uiManager.UpdateNotificationQueue("Bought a drug");
            animationController.TriggerYes();
        }
        else
        {
            // Show error messages of failed purchase
            uiManager.UpdateNotificationQueue("Not enough samples");
            animationController.TriggerNo();
        }
    }
    public void enterExitStudyDungeon()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        // Switch between 'StudyDungeon' or 'Village'
        SceneManager.LoadScene(currentSceneName == "StudyDungeon" ? "Village" : "StudyDungeon");
    }
    public void study()
    {
        return;
    }
}

