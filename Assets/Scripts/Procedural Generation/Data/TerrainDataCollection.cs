using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainDataCollection
{
    public int phytomassThreshold;
    public TerrainData terrain;
    public NoiseData noise;
    public TextureData texture;
    public FloraData flora;
}
