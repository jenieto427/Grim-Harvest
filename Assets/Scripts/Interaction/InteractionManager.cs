using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private UIManager uiManager;
    private NPCWizardAnimationController animationController;


    private void Awake()
    {
        GameObject uiManagerObject = GameObject.Find("UIManager");
        uiManager = uiManagerObject.GetComponent<UIManager>();
        GameObject npcObject = GameObject.Find("Wizard"); // Replace "NPC" with your NPC's name in the scene
        animationController = npcObject.GetComponent<NPCWizardAnimationController>();

    }

    public void HandleVendorInteraction(Player player, GameObject vendor)
    {
        int cost = 5; // Cost in plant materials for one stimulant
        if (player.plantMaterial >= cost)
        {
            player.DecreasePlantMaterial(cost); // Reduce samples/money
            player.IncrementStimulant(); // Increment stimulant count 
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
}

