using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private PlayerDataManager playerDataManager;
    void Start()
    {
        playerDataManager = GameObject.Find("PlayerDataManager").GetComponent<PlayerDataManager>();
        // Check if this is the Village scene and the tutorial hasn't been seen
        if (SceneManager.GetActiveScene().name == "Village" && playerDataManager.seenTutorial == 0)
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}
