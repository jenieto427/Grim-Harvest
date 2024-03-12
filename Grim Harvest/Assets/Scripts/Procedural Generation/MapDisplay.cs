using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * CODE BASED ON IMPLEMENTATION BY SEBASTIAN LAGUE
 * FROM HIS PLAYLIST ON YOUTUBE "PROCEDURAL TERRAIN GENERATION"
 */


public class MapDisplay : MonoBehaviour
{
	public Renderer textureRenderer;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
	
	public void DrawTexture(Texture2D texture)
	{
		//Generate object in scene
		textureRenderer.sharedMaterial.mainTexture = texture;
		textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
	}

	public void DrawMesh(MeshData meshData, Texture2D texture)
	{
		meshFilter.sharedMesh = meshData.CreateMesh();
		meshRenderer.sharedMaterial.mainTexture = texture;
	}
}
