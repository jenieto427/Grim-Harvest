using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * CODE BASED ON IMPLEMENTATION BY SEBASTIAN LAGUE
 * FROM HIS PLAYLIST ON YOUTUBE "PROCEDURAL TERRAIN GENERATION"
 */

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap, Mesh}
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    [Range(0f, 25f)]
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    [Range(0, 16)]
    public int octaves;
    [Range(0f, 1f)]
    public float persistence;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        //Retrieve perlin noise array
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistence, lacunarity, offset);

        //Apply colors
        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++ )
        {
            for (int x = 0; x < mapWidth; x++ )
            {
                float currentHeight = noiseMap[x, y];

                foreach (var region in regions)
                {
                    if (currentHeight <= region.heightThreshold)
                    {
                        colorMap[y * mapWidth + x] = region.terrainColor;
                        break;
                    }
                }
            }
        }

        //Find and update map with noise map
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
		}
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }

    }

    //Called whenever variable in editor changed
	private void OnValidate()
	{
        /*
         * Clamp all values
         */

		if (mapWidth < 1 )
        {
            mapWidth = 1;
        }

        if (mapHeight < 1 ) 
        {
            mapHeight = 1;
        }

        if (lacunarity < 1 )
        {
            lacunarity = 1;
        }

        if (octaves < 0 )
        {
            octaves = 0;
        }
	}
}

[System.Serializable]
public struct TerrainType
{
    public string terrainName;
    public float heightThreshold;
    public Color terrainColor;


}
