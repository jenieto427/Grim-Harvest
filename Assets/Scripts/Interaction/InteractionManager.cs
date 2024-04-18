using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HandleVendorInteraction(Player player, GameObject vendor)
    {
        int cost = 5; // Cost in plant materials for one stimulant
        if (player.plantMaterial >= cost)
        {
            player.DecreasePlantMaterial(cost); // Reduce samples/money
            player.IncrementStimulant(); // Increment stimulant count 
            // Show success message
            UIManager.Instance.UpdateNotificationQueue("Bought stimulant");
        }
        else
        {
            // Show error message of failed purchase
            UIManager.Instance.UpdateNotificationQueue("You don't have enough samples");
        }
    }
}

