using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Include this for UI components

public class Options : MonoBehaviour
{
   public static string prevSceneName;
   public Slider mouseSensitivitySlider; // Slider for adjusting mouse sensitivity
   private PlayerDataManager playerDataManager; // Reference to the PlayerDataManager

   private void Start()
   {
      // Find the PlayerDataManager in the scene
      playerDataManager = FindObjectOfType<PlayerDataManager>();
      // Initialize the slider value
      if (playerDataManager != null && mouseSensitivitySlider != null)
      {
         mouseSensitivitySlider.value = playerDataManager.mouseSensitivity;
         mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
      }
   }

   public static void setPrevSceneName(string str)
   {
      prevSceneName = str;
   }

   public void back()
   {
      SceneManager.LoadScene(prevSceneName);
   }

   // Method to update mouse sensitivity from the slider
   private void SetMouseSensitivity(float sensitivity)
   {
      if (playerDataManager != null)
      {
         playerDataManager.setMouseSensitivity(sensitivity);
      }
   }
}
