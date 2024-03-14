using System;
using System.Threading;
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

    public const int mapChunkSize = 239;
    [Range(0, 6)]
    public int editorPreviewLOD;
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

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);

		//Find and update map with noise map
		MapDisplay display = FindObjectOfType<MapDisplay>();
		if (drawMode == DrawMode.NoiseMap)
		{
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
		}
		else if (drawMode == DrawMode.ColorMap)
		{
			display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.coulourMap, mapChunkSize, mapChunkSize));
		}
		else if (drawMode == DrawMode.Mesh)
		{
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPreviewLOD), TextureGenerator.TextureFromColorMap(mapData.coulourMap, mapChunkSize, mapChunkSize));
		}
	}

    public void RequestMapData(Vector2 center, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(center, callback);
        };

        new Thread(threadStart).Start();
    }

    void MapDataThread(Vector2 center, Action<MapData> callback)
    {
        MapData mapData = GenerateMapData(center);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, lod, callback);
        };

        new Thread(threadStart).Start();
    }

    void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback) 
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod);
        lock(meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }

	void Update()
	{
		if (mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        if (meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0;i < meshDataThreadInfoQueue.Count;i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
	}

	MapData GenerateMapData(Vector2 center)
    {
        //Retrieve perlin noise array
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, seed, noiseScale, octaves, persistence, lacunarity, center + offset);

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

        return new MapData(noiseMap, colorMap);
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

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T paramater)
        {
            this.callback = callback;
            this.parameter = paramater;
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

public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] coulourMap;

    public MapData(float[,] heightMap, Color[] colourMap)
    {
        this.heightMap = heightMap;
        this.coulourMap = colourMap;
    }
}