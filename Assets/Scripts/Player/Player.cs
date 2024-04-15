using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int energy = 100;
    public int plantMaterial = 0;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(This);
    }
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        energy = data.energy;
        plantMaterial = data.plantMaterial;

        Vector3.position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        //transform.position = position; //Activate when the scene loading is handled.
    }
}
