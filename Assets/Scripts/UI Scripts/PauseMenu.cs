using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public static bool optionsIsUp = false;
    public GameObject pauseMenuUI;
    //public GameObject optionsMenuUI;

    //public GameObject cameraController;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("pause");
            if(gameIsPaused)
            {
                Resume();
            }
            else if (optionsIsUp)
            {
                BackToPauseMenu();
            }
            else
            {
                Pause();
            }
        }

        else if (Input.GetKeyDown(KeyCode.M) && gameIsPaused)
        {
            LoadMenu();
        }

        else if (Input.GetKeyDown(KeyCode.O) && gameIsPaused)
        {
            Options();

        }

        else if (Input.GetKeyDown(KeyCode.Q) && gameIsPaused)
        {
            QuitGame();
        }

        else if(Input.GetKeyDown(KeyCode.Y) && optionsIsUp)
        {
       // Component cc = cameraController.GetComponent<CameraLookController>;
           
        }
        
    }

    public void Resume() 
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        Debug.Log("MainMenu load");
        SceneManager.LoadScene("MainMenu");
    }

    public void Options()
    {
        //Options.setPrevSceneName("MapGenerationTest");
        //SceneManager.LoadScene("Options");
        pauseMenuUI.SetActive(false);
        //optionsMenuUI.SetActive(true);
        optionsIsUp = true;
    }

    public void BackToPauseMenu()
    {
        //optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        optionsIsUp = false;

    }



    public void QuitGame()
    {
        Application.Quit();
       // UnityEditor.EditorApplication.isPlaying = false;
    }
}
