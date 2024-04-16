using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Image fadeImage;
    public Player player;
    public TextMeshProUGUI wakefulText;  // UI element for energy
    public TextMeshProUGUI plantMaterialText;  // UI element for plant material
    public TextMeshProUGUI stimulantText;  // UI element for plant material

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
        if (player != null)
        {
            // Update text elements with the current player attributes
            if (player.energy > 13f) //Player is wakeful at 13KHz
            {
                wakefulText.text = "Beta Wave: " + player.energy.ToString() + " Hz";
            }
            else if (player.energy >= 8f) //Player is sleepy below 13KHz to 8KHz
            {
                wakefulText.text = "Alpha Wave: " + player.energy.ToString() + " Hz";
                //TODO: Notification to player that they are sleepy
            }
            else if (player.energy >= 4f)
            {
                wakefulText.text = "Theta Wave: " + player.energy.ToString() + " Hz";
            }
            else if (player.energy >= 1f)
            {
                wakefulText.text = "Delta Wave: " + player.energy.ToString() + " Hz";
            }
            else if (player.energy < 1f)
            {
                wakefulText.text = "Below Delta: " + player.energy.ToString() + " Hz";
            }

            plantMaterialText.text = "Data & Plant Materials: " + player.plantMaterial.ToString();
            stimulantText.text = "Stimulants: " + player.stimulant.ToString();
        }
    }

    public IEnumerator FadeToBlack(float fadeDuration = 1f)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
    }

    public IEnumerator FadeFromBlack(float fadeDuration = 1f)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
    }
}
