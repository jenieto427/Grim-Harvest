using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialSlideshow : MonoBehaviour
{
    public Image[] panels;
    private int currentIndex = 0;

    void Start()
    {
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
            // Load Village scene when end of panels is reached or next on the last panel
            SceneManager.LoadScene("Village");
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
        SceneManager.LoadScene("Village");
    }

    private void ShowPanel(int index)
    {
        foreach (Image panel in panels)
        {
            panel.gameObject.SetActive(false);
        }
        panels[index].gameObject.SetActive(true);
    }
}
