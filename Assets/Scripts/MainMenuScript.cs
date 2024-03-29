using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            startbtn();
        }

        else if (Input.GetKeyDown(KeyCode.O))
        {
            optionsBtn();
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitBtn();
        }

    }
    public void startbtn()
    {
        SceneManager.LoadScene("MapGenerationTest");
    }
    
    public void optionsBtn()
    {
        Options.setPrevSceneName(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Options");
    }

    public void quitBtn()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

