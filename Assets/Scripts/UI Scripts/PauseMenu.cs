using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public static bool optionsIsUp = false;
    public GameObject pauseMenuUI;
    public PlayerDataManager playerDataManager;
    //public GameObject optionsMenuUI;

    //public GameObject cameraController;
    void Awake()
    {
        playerDataManager = GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("pause");
            if (gameIsPaused)
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

    public void LoadBtn()
    {
        Debug.Log("Load");
        playerDataManager.LoadPlayerPrefs();
    }

    public void SaveBtn()
    {
        Debug.Log("Save");
        playerDataManager.SavePlayerPrefs();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
