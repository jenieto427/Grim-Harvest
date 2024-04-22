using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool inventoryIsUp;
    public GameObject inventoryUI;
    //


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryIsUp == false)
            {
                OpenInvetory();
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

    public void OpenInvetory()
    {
        inventoryUI.SetActive(true);
        Time.timeScale = 0f;
        inventoryIsUp = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        inventoryUI.SetActive(false);
        Time.timeScale = 1f;
        inventoryIsUp = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


}
