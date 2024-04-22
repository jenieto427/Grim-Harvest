using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainGeneratorFactory : MonoBehaviour
{
    public GameObject playerPrefab;
	/// <summary>
	/// SET PLAYER TRANSFORM
	/// </summary>

    //Terrain Objects
    public Material terrainMaterial;
    public NoiseData noiseDataDefault;
    public FloraData floraDataDefault;
    public List<TerrainDataCollection> environmentDataSheets;

	//Object parents
	public GameObject generatorPrefab;
    public GameObject treeParent;
    public GameObject plantParent;

    //Map Generator constants
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
		mapHasGenerated = true;

		//Instantiate new generator
		GameObject terrainGenerator = Instantiate(generatorPrefab, Vector3.zero, Quaternion.identity);
		MapGenerator mapGenerator = terrainGenerator.GetComponentInChildren<MapGenerator>();
		EndlessTerrain endlessTerrain = terrainGenerator.GetComponentInChildren<EndlessTerrain>();

		//Set as child of factory
		GameObject factory = GameObject.FindGameObjectWithTag("Factory");
		terrainGenerator.transform.parent = factory.transform;

		//Set Endless Terrain and Map Generator Values
		SetConstantValues(mapGenerator, endlessTerrain);

		//Determine Texture Data Sheets
		DetermineTextureDataSheets(mapGenerator);

		//Run Map Generator
		mapGenerator.OnSceneLoad();
		endlessTerrain.OnSceneLoad();

		endlessTerrain.mapGeneratorFinished = true;
	}

	void SetConstantValues(MapGenerator mapGenerator, EndlessTerrain endlessTerrain)
	{
		//Map Generator Values
		mapGenerator.drawMode = DRAW_MODE;
		mapGenerator.noiseHeightEstimation = NOISE_HEIGHT_ESTIMATION;
		mapGenerator.editorPreviewLOD = EDITOR_PREVIEW_LOD;
		mapGenerator.autoUpdate = AUTO_UPDATE;

		//Endless Terrain Values
		endlessTerrain.viewer = playerPrefab.transform;
		endlessTerrain.floraData = floraDataDefault;
		endlessTerrain.treeParent = treeParent;
		endlessTerrain.plantParent = plantParent;
	}

	void DetermineTextureDataSheets(MapGenerator terrainGenerator) {
		int phytomass = playerPrefab.GetComponentInChildren<Player>().phytomass;
		TerrainDataCollection dataSheet = null;

		foreach (TerrainDataCollection sheet in environmentDataSheets)
		{
			if (phytomass >= sheet.phytomassThreshold)
			{
				//Ensure it's the highest possible sheet
				if (dataSheet != null)
				{
					if (sheet.phytomassThreshold > dataSheet.phytomassThreshold)
					{
						dataSheet = sheet;
					}
				} else
				{
					dataSheet = sheet;
				}
			}
		}

		if (dataSheet != null)
		{
			SetTextureData(terrainGenerator, dataSheet);
		}
		else
		{
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