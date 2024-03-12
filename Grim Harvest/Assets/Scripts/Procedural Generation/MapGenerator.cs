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

    const int mapChunkSize = 241;
    [Range(0, 6)]
    public int levelOfDetail;
    public float noiseScale;

    [Range(0f, 25f)]
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    [Range(0, 16)]
    public int octaves;
    [Range(0f, 1f)]
    public float persistence;
    [Range(0f, 11f)]
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        //Retrieve perlin noise array
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistence, lacunarity, offset);

        //Apply colors
        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++ )
        {
            for (int x = 0; x < mapChunkSize; x++ )
            {
                float currentHeight = noiseMap[x, y];

                foreach (var region in regions)
                {
                    if (currentHeight <= region.heightThreshold)
                    {
                        colorMap[y * mapChunkSize + x] = region.terrainColor;
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
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }

    }

    //Called whenever variable in editor changed
	private void OnValidate()
	{
        /*
         * Clamp all values
         */
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
