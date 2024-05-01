using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;

/* 
 * CODE BASED ON IMPLEMENTATION BY SEBASTIAN LAGUE
 * FROM HIS PLAYLIST ON YOUTUBE "PROCEDURAL TERRAIN GENERATION"
 */

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, Mesh }
    public DrawMode drawMode;

    public TerrainData terrainData;
    public NoiseData noiseData;
    public TextureData textureData;

    public Material terrainMaterial;

    [Range(1f, 2f)]
    public float noiseHeightEstimation;

    [Range(0, 6)]
    public int editorPreviewLOD;

    public bool editorMapIsEnabled = false;

    public bool autoUpdate;

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public void OnSceneLoad()
    {
        textureData.UpdateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);
    }

    public int mapChunkSize
    {
        get
        {
            return (terrainData.useFlatShading) ? 95 : 239;
        }
    }

    void OnValuesUpdated()
    {
        textureData.ApplyToMaterial(terrainMaterial);

        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    void OnTextureValuesUpdated()
    {

    }

    public void DrawMapInEditor()
    {
        if (!editorMapIsEnabled) return;

        // Check for null on textureData and terrainMaterial
        if (textureData == null)
        {
            Debug.LogError("TextureData is null.");
            return;
        }

        if (terrainMaterial == null)
        {
            Debug.LogError("TerrainMaterial is null.");
            return;
        }

        if (terrainData == null)
        {
            Debug.LogError("TerrainData is null.");
            return;
        }

        textureData.UpdateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);

        MapData mapData = GenerateMapData(Vector2.zero);

        // Ensure MapDisplay is found
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (display == null)
        {
            Debug.LogError("No MapDisplay found in the scene.");
            return;
        }

        // Drawing based on drawMode
        if (drawMode == DrawMode.NoiseMap)
        {
            if (mapData.heightMap == null)
            {
                Debug.LogError("HeightMap is null.");
                return;
            }
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            if (terrainData.meshHeightCurve == null)
            {
                Debug.LogError("MeshHeightCurve is null.");
                return;
            }
            MeshData generatedMesh = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD, terrainData.useFlatShading);
            if (generatedMesh == null)
            {
                Debug.LogError("GeneratedMesh is null.");
                return;
            }
            display.DrawMesh(generatedMesh);
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
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, lod, terrainData.useFlatShading);
        lock (meshDataThreadInfoQueue)
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
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    MapData GenerateMapData(Vector2 center)
    {
        //Retrieve perlin noise array
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistence, noiseData.lacunarity, center + noiseData.offset, noiseData.normalizeMode);

        return new MapData(noiseMap);
    }

    void OnValidate()
    {
        if (terrainData != null)
        {
            terrainData.OnValuesUpdated -= OnValuesUpdated;
            terrainData.OnValuesUpdated += OnValuesUpdated;
        }

        if (noiseData != null)
        {
            noiseData.OnValuesUpdated -= OnValuesUpdated;
            noiseData.OnValuesUpdated += OnValuesUpdated;
        }

        if (textureData != null)
        {
            textureData.OnValuesUpdated -= OnValuesUpdated;
            textureData.OnValuesUpdated += OnValuesUpdated;
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

public struct MapData
{
    public readonly float[,] heightMap;

    public MapData(float[,] heightMap)
    {
        this.heightMap = heightMap;
    }
}