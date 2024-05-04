using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorFactory : MonoBehaviour
{
	public GameObject playerPrefab;
	public Material terrainMaterial;
	public NoiseData noiseDataDefault;
	public FloraData floraDataDefault;
	public List<TerrainDataCollection> environmentDataSheets;
	public GameObject generatorPrefab;
	public GameObject treeParent;
	public GameObject plantParent;

	const MapGenerator.DrawMode DRAW_MODE = MapGenerator.DrawMode.Mesh;
	const float NOISE_HEIGHT_ESTIMATION = 1.12f;
	const int EDITOR_PREVIEW_LOD = 0;
	const bool AUTO_UPDATE = true;

	bool mapHasGenerated = false;

	private void Update()
	{
		if (!mapHasGenerated)
		{
			InstantiateTerrainGenerator();
		}
	}

	public void InstantiateTerrainGenerator()
	{
		GameObject terrainGenerator = Instantiate(generatorPrefab, Vector3.zero, Quaternion.identity);
		MapGenerator mapGenerator = terrainGenerator.GetComponentInChildren<MapGenerator>();
		EndlessTerrain endlessTerrain = terrainGenerator.GetComponentInChildren<EndlessTerrain>();
		MapDisplay mapDisplay = terrainGenerator.GetComponentInChildren<MapDisplay>();

		if (mapGenerator == null || endlessTerrain == null || mapDisplay == null)
		{
			Debug.LogError("Required components are missing on the terrain generator prefab.");
			Destroy(terrainGenerator); // Cleanup if not correctly set up.
			return;
		}

		mapGenerator.SetMapDisplay(mapDisplay); // Set the MapDisplay reference
		mapGenerator.editorMapIsEnabled = true; // Ensure editor map is enabled

		terrainGenerator.transform.SetParent(GameObject.FindGameObjectWithTag("Factory")?.transform ?? this.transform, false);

		SetConstantValues(mapGenerator, endlessTerrain);
		DetermineTextureDataSheets(mapGenerator);

		mapGenerator.OnSceneLoad();
		endlessTerrain.OnSceneLoad();

		endlessTerrain.mapGeneratorFinished = true;
		mapHasGenerated = true;
	}

	void SetConstantValues(MapGenerator mapGenerator, EndlessTerrain endlessTerrain)
	{
		mapGenerator.drawMode = DRAW_MODE;
		mapGenerator.noiseHeightEstimation = NOISE_HEIGHT_ESTIMATION;
		mapGenerator.editorPreviewLOD = EDITOR_PREVIEW_LOD;
		mapGenerator.autoUpdate = AUTO_UPDATE;

		endlessTerrain.viewer = playerPrefab.transform;
		endlessTerrain.floraData = floraDataDefault;
		endlessTerrain.treeParent = treeParent;
		endlessTerrain.plantParent = plantParent;
	}

	void DetermineTextureDataSheets(MapGenerator terrainGenerator)
	{
		PlayerDataManager playerDataManager = FindObjectOfType<PlayerDataManager>();
		if (playerDataManager == null)
		{
			Debug.LogError("PlayerDataManager not found in the scene.");
			return;
		}

		int phytomass = playerDataManager.phytomass;
		TerrainDataCollection dataSheet = null;

		foreach (TerrainDataCollection sheet in environmentDataSheets)
		{
			if (phytomass >= sheet.phytomassThreshold && (dataSheet == null || sheet.phytomassThreshold > dataSheet.phytomassThreshold))
			{
				dataSheet = sheet;
			}
		}

		if (dataSheet != null)
		{
			dataSheet.disableAutoUpdate();
			SetTextureData(terrainGenerator, dataSheet);
		}
		else
		{
			Debug.LogError("No valid data sheet found; using default.");
			SetTextureData(terrainGenerator, environmentDataSheets[0]);
		}
	}

	void SetTextureData(MapGenerator terrainGenerator, TerrainDataCollection dataSheet)
	{
		terrainGenerator.terrainData = dataSheet.terrain;
		terrainGenerator.textureData = dataSheet.texture;
		terrainGenerator.noiseData = noiseDataDefault;
		terrainGenerator.terrainMaterial = terrainMaterial;
	}
}
