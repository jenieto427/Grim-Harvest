using UnityEngine;
using UnityEditor;

/* 
 * CODE BASED ON IMPLEMENTATION BY SEBASTIAN LAGUE
 * FROM HIS PLAYLIST ON YOUTUBE "PROCEDURAL TERRAIN GENERATION"
 */


//Indicates to show button on MapGenerator Components
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		//Cast target being inspected to MapGenerator
		MapGenerator mapGen = (MapGenerator) target;

		//If any value is changed
		if (DrawDefaultInspector())
		{
			if (mapGen.autoUpdate)
			{
				mapGen.GenerateMap();
			}
		}

		//Create button for map generation
		if (GUILayout.Button("Generate Map"))
		{
			mapGen.GenerateMap();
		}
	}
}

