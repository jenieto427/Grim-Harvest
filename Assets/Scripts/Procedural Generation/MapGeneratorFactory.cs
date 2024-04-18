using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MapGeneratorFactory : ScriptableObject
{
    //TODO: DYNAMICLY ASSIGNING DATA SHEETS
    public Player player;
    public Material terrainMaterial;
    public List<TerrainDataCollection> environmentDataSheets;

    public MapGenerator InitMapGenerator() {
        MapGenerator newGenerator = new MapGenerator();

        int phytomass = player.phytomass;
        TerrainDataCollection dataSheet = null;

        foreach (TerrainDataCollection sheet in environmentDataSheets) {
            if (phytomass >= sheet.phytomassThreshold) {
                dataSheet = sheet;
                break;
            }
        }

        if (dataSheet != null) {
            SetData(newGenerator, dataSheet);
        } else {
            SetData(newGenerator, environmentDataSheets[0]);
        }

        return newGenerator;
    }

    void SetData(MapGenerator generator, TerrainDataCollection dataSheet) {
        generator.terrainData = dataSheet.terrain;
        generator.textureData = dataSheet.texture;
        generator.noiseData = dataSheet.noise;
        generator.terrainMaterial = terrainMaterial;
    }
}