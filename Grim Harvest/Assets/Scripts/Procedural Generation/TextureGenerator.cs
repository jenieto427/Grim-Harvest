using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * CODE BASED ON IMPLEMENTATION BY SEBASTIAN LAGUE
 * FROM HIS PLAYLIST ON YOUTUBE "PROCEDURAL TERRAIN GENERATION"
 */


public static class TextureGenerator
{
	public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
	{
		Texture2D texture = new Texture2D(width, height);
		//Fix noise bluriness
		texture.filterMode = FilterMode.Point;
		//Stop noise wrapping
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(colorMap);
		texture.Apply();
		return texture;
	}

	public static Texture2D TextureFromHeightMap(float[,] heightMap)
	{
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);

		//Generate grayscale from noise map height
		Color[] colorMap = new Color[width * height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
			}
		}

		return TextureFromColorMap(colorMap, width, height);
	}
}
