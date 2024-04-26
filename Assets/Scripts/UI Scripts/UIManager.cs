using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private PlayerDataManager playerDataManager;
    public TextMeshProUGUI wakefulText;  // UI element for energy
    public TextMeshProUGUI plantMaterialText;  // UI element for plant material
    public TextMeshProUGUI stimulantText;  // UI element for plant material
    public TextMeshProUGUI phytomassText; // UI element for phytomass
    public TextMeshProUGUI notificationText; // UI element for notications
    public TextMeshProUGUI interactionText; // UI element for interactions
    private Queue<string> notificationQueue = new Queue<string>(); // Notification string queue
    private string lastInteractionText = "";

    private void Awake()
    {
        playerDataManager = GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        UpdatePlayerUI();
    }
    // Notification system
    public void UpdateNotificationUI()
    {
        notificationText.text = string.Join("\n", notificationQueue.ToArray());
    }
    public void UpdateNotificationQueue(string notificationString)
    {
        // Add new notification to the queue
        notificationQueue.Enqueue(notificationString);

        // Check if the queue size has exceeded the limit
        if (notificationQueue.Count > 3)
        {
            notificationQueue.Dequeue(); // Remove the oldest notification
        }

        // Update the TextMeshPro text
        UpdateNotificationUI();
    }
    // For Interactions through the raycast reticle
    public void UpdateInteractionUI(String interactionString)
    {
        if (interactionString != lastInteractionText)
        {
            lastInteractionText = interactionString;
            interactionText.text = interactionString;
            // Optionally, force a canvas update if needed
            Canvas.ForceUpdateCanvases();
        }
        //interactionText.text = interactionString;
    }
    public void UpdatePlayerUI()
    {
        if (playerDataManager != null)
        {
            // Update text elements with the current player attributes
            if (playerDataManager.energy > 13f) //Player is wakeful at 13KHz
            {
                wakefulText.text = "Beta Activity: " + playerDataManager.energy.ToString() + " Hz";
            }
            else if (playerDataManager.energy >= 8f) //Player is sleepy below 13KHz to 8KHz
            {
                wakefulText.text = "Alpha Activity: " + playerDataManager.energy.ToString() + " Hz";
                //TODO: Notification to player that they are sleepy
            }
            else if (playerDataManager.energy >= 4f)
            {
                wakefulText.text = "Theta Activity: " + playerDataManager.energy.ToString() + " Hz";
            }
            else if (playerDataManager.energy >= 1f)
            {
                wakefulText.text = "Delta Activity: " + playerDataManager.energy.ToString() + " Hz";
            }
            else if (playerDataManager.energy < 1f)
            {
                wakefulText.text = "Below Delta: " + playerDataManager.energy.ToString() + " Hz";
            }

            plantMaterialText.text = "Data & Plant Materials: " + playerDataManager.plantMaterial.ToString() + " samples";
            stimulantText.text = "Stimulants: " + playerDataManager.stimulant.ToString();
            phytomassText.text = "Phytomass: " + playerDataManager.phytomass.ToString();

        }
    }
}
