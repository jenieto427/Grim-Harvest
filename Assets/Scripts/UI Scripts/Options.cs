using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
   public static string prevSceneName;

   public static void setPrevSceneName(string str)
   {
    prevSceneName = str;
   }

   public void back()
   {
    SceneManager.LoadScene(prevSceneName);
   }
   
}
