using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ComicSlideshow : MonoBehaviour
{
    public Image[] panels;
    private int currentIndex = 0;

    void Start()
    {
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
            SceneManager.LoadScene("Tutorial");
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
        SceneManager.LoadScene("Tutorial");
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
