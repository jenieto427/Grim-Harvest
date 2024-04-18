using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private const string VILLAGE_SCENE_NAME = "Village";
    private const string MAP_SCENE_NAME = "MapGenerationTest";

    public static void LoadScene(string SCENE_NAME) {
        EndlessTerrain terrainScript = GameObject.FindObjectOfType<EndlessTerrain>();

        switch (SCENE_NAME)
        {
            case VILLAGE_SCENE_NAME:
                terrainScript.ClearTerrain();
                SceneManager.LoadScene(VILLAGE_SCENE_NAME);
                break;

            case MAP_SCENE_NAME:
                //Load Scene
                SceneManager.LoadScene(MAP_SCENE_NAME);

                //Factorize (lol) new MapGenerator
                MapGeneratorFactory factory = new MapGeneratorFactory();
                MapGenerator mapGenerator = factory.InitMapGenerator();

                break;

            default:
                Debug.Log("Scene Not Found.");
                break;
        }
    }
}
