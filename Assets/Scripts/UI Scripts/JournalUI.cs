using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class JournalUI : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool inventoryIsUp;
    public GameObject journal;
    //


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            if(inventoryIsUp == false)
            {
                OpenJournal();
            }
            
            else if(inventoryIsUp == true)
            {
                Close();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(inventoryIsUp == true)
            {
                Close();
            }
        }
    }

    public void OpenJournal()
    {
        journal.SetActive(true);
        Time.timeScale = 0f;
        inventoryIsUp = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        journal.SetActive(false);
        Time.timeScale = 1f;
        inventoryIsUp = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


}
