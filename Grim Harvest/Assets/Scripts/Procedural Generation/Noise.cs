using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * CODE BASED ON IMPLEMENTATION BY SEBASTIAN LAGUE
 * FROM HIS PLAYLIST ON YOUTUBE "PROCEDURAL TERRAIN GENERATION"
 */

public static class Noise 
{
    /*
     * Generate a noise map for land generation height
     */
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        //Generates random sample points for randomized noise maps
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        //Clamp scale to value above 0
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        //Keep track of max and min height values for normalization
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        //Map scale will zoom towards center of map
        float halfMapWidth = mapWidth / 2f;
        float halfMapHeight = mapHeight / 2f;

        //Calculate each noise height value
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    //Choose sample points
                    //Frequency determines how rapidly the heights of the noisemap change
                    float sampleX = (x - halfMapWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfMapHeight) / scale * frequency + octaveOffsets[i].y;

                    //Generate land height
                    //float perlinValue = Mathf.PerlinNoise(sampleX, sampleY); //Only allows positive changes in land height
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; //Allows for negative changes in land height

                    noiseHeight += perlinValue * amplitude;

                    //Amplitude decreases each octave
                    amplitude *= persistance;
                    //Frequency increases each octave
                    frequency *= lacunarity;
                }

                //Update max and min noise height trackers
                if(noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                if(noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;

                //Set noise height value
                noiseMap[x, y] = noiseHeight;
            }
        }

		//Normalize each bit of noise to be between 0-1
		for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }


		return noiseMap;
    }
}
