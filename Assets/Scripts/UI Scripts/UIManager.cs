using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Image fadeImage;

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
