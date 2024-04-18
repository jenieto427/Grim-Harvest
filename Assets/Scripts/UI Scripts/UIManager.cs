using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Player player;
    public TextMeshProUGUI wakefulText;  // UI element for energy
    public TextMeshProUGUI plantMaterialText;  // UI element for plant material
    public TextMeshProUGUI stimulantText;  // UI element for plant material
    public TextMeshProUGUI notificationText; // UI element for notications
    public TextMeshProUGUI interactionText; // UI element for interactions
    private Queue<string> notificationQueue = new Queue<string>(); // Notification string queue

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
        interactionText.text = interactionString;
    }
    public void UpdatePlayerUI()
    {
        if (player != null)
        {
            // Update text elements with the current player attributes
            if (player.energy > 13f) //Player is wakeful at 13KHz
            {
                wakefulText.text = "Beta Activity: " + player.energy.ToString() + " Hz";
            }
            else if (player.energy >= 8f) //Player is sleepy below 13KHz to 8KHz
            {
                wakefulText.text = "Alpha Activity: " + player.energy.ToString() + " Hz";
                //TODO: Notification to player that they are sleepy
            }
            else if (player.energy >= 4f)
            {
                wakefulText.text = "Theta Activity: " + player.energy.ToString() + " Hz";
            }
            else if (player.energy >= 1f)
            {
                wakefulText.text = "Delta Activity: " + player.energy.ToString() + " Hz";
            }
            else if (player.energy < 1f)
            {
                wakefulText.text = "Below Delta: " + player.energy.ToString() + " Hz";
            }

            plantMaterialText.text = "Data & Plant Materials: " + player.plantMaterial.ToString() + " samples";
            stimulantText.text = "Stimulants: " + player.stimulant.ToString();
        }
    }
}
