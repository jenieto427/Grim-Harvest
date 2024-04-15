using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public int hunger;
    public int resources;
    public float[] position;

    //Constructor
    public PlayerData(Player player)
    {
        hunger = player.hunger;
        resources = player.resources;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
