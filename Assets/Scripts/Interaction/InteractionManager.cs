using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private UIManager uiManager;

    private void Awake()
    {
        GameObject uiManagerObject = GameObject.Find("UIManager");
        uiManager = uiManagerObject.GetComponent<UIManager>();
    }

    public void HandleVendorInteraction(Player player, GameObject vendor)
    {
        int cost = 5; // Cost in plant materials for one stimulant
        if (player.plantMaterial >= cost)
        {
            player.DecreasePlantMaterial(cost); // Reduce samples/money
            player.IncrementStimulant(); // Increment stimulant count 
            // Show success message
            uiManager.UpdateNotificationQueue("Bought a drug");
        }
        else
        {
            // Show error message of failed purchase
            uiManager.UpdateNotificationQueue("Not enough samples");
        }
    }
}

