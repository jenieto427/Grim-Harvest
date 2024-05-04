using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

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

    private MapDisplay mapDisplay;
    private Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    private Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public void SetMapDisplay(MapDisplay display)
    {
        mapDisplay = display;
        Debug.Log("MapDisplay set successfully.");
    }

    public void EnableEditorMap(bool enable)
    {
        editorMapIsEnabled = enable;
        Debug.Log($"Editor map enabled state set to: {enable}");
    }

    public void OnSceneLoad()
    {
        if (textureData != null)
            textureData.UpdateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);
    }

    public int mapChunkSize
    {
        get { return (terrainData.useFlatShading) ? 95 : 239; }
    }

    void OnValuesUpdated()
    {
        if (terrainMaterial != null && textureData != null)
            textureData.ApplyToMaterial(terrainMaterial);

        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    public void DrawMapInEditor()
    {
        if (!editorMapIsEnabled || mapDisplay == null)
        {
            Debug.LogError("Editor map is disabled or MapDisplay is not set.");
            return;
        }

        if (textureData == null || terrainMaterial == null || terrainData == null)
        {
            Debug.LogError("One or more essential components (TextureData, TerrainMaterial, TerrainData) are not set.");
            return;
        }

        textureData.UpdateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);
        MapData mapData = GenerateMapData(Vector2.zero);

        if (drawMode == DrawMode.NoiseMap && mapData.heightMap != null)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.Mesh && terrainData.meshHeightCurve != null)
        {
            MeshData generatedMesh = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD, terrainData.useFlatShading);
            if (generatedMesh != null)
                mapDisplay.DrawMesh(generatedMesh);
            else
                Debug.LogError("Failed to generate mesh data.");
        }
        else
        {
            Debug.LogError("Invalid draw mode or missing height map/mesh height curve.");
        }
    }

    public void RequestMapData(Vector2 center, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate { MapDataThread(center, callback); };
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
        ThreadStart threadStart = delegate { MeshDataThread(mapData, lod, callback); };
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
        ProcessThreadInfoQueue(mapDataThreadInfoQueue);
        ProcessThreadInfoQueue(meshDataThreadInfoQueue);
    }

    void ProcessThreadInfoQueue<T>(Queue<MapThreadInfo<T>> queue)
    {
        while (queue.Count > 0)
        {
            MapThreadInfo<T> threadInfo = queue.Dequeue();
            threadInfo.callback(threadInfo.parameter);
        }
    }

    MapData GenerateMapData(Vector2 center)
    {
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

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
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
