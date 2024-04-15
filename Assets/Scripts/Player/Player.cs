using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int hunger = 50;
    public int resources = 0;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(This);
    }
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        hunger = data.hunger;
        resources = data.resources;

        Vector3.position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        //transform.position = position; //Activate when the scene loading is handled.
    }
}
