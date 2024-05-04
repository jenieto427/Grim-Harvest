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

    public void disableAutoUpdate()
    {
        if (terrain != null)
            terrain.autoUpdate = false;
        if (noise != null)
            noise.autoUpdate = false;
        if (texture != null)
            texture.autoUpdate = false;
        if (flora != null)
            flora.autoUpdate = false;
    }
}
