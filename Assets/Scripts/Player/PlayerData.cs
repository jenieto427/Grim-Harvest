using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public static PlayerData current;
    public float energy;
    public int plantMaterial;
    public int stimulant;
    public float[] position;

    //Constructor
    public PlayerData(Player player)
    {
        energy = player.energy;
        plantMaterial = player.plantMaterial;
        stimulant = player.stimulant;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        current = this;
    }
}
