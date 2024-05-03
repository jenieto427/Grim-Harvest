using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialSlideshow : MonoBehaviour
{
    public Image[] panels;
    public GameObject slideshowCanvas; // Assign the canvas in the inspector
    private PlayerDataManager playerDataManager;
    private int currentIndex = 0;

    void Start()
    {
        playerDataManager = GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ShowPanel(currentIndex);
    }

    public void NextPanel()
    {
        if (currentIndex + 1 < panels.Length)
        {
            ShowPanel(++currentIndex);
        }
        else
        {
            FinishSlideshow();
        }
    }
    public void PreviousPanel()
    {
        if (currentIndex > 0)
        {
            ShowPanel(--currentIndex);
        }
    }

    public void SkipToEnd()
    {
        FinishSlideshow();
    }

    private void ShowPanel(int index)
    {
        foreach (Image panel in panels)
        {
            panel.gameObject.SetActive(false);
        }
        panels[index].gameObject.SetActive(true);
    }

    private void FinishSlideshow()
    {
        playerDataManager.SetSeenTutorialTrue(); // Assuming this function sets seenTutorial to 1
        SceneManager.LoadScene("Village");
    }
}
