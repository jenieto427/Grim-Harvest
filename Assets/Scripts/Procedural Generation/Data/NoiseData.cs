using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseData : UpdatableData
{
	public Noise.NormalizeMode normalizeMode;

	public float noiseScale;

	[Range(0, 16)]
	public int octaves;

	[Range(0f, 1f)]
	public float persistence;

	[Range(0f, 2.5f)]
	public float lacunarity;

	public Vector2 offset;

	public int seed;
	#if UNITY_EDITOR
	protected override void OnValidate()
	{
		if (lacunarity < 1)
		{
			lacunarity = 1;
		}

		if (octaves < 1)
		{
			octaves = 1;
		}
		#if UNITY_EDITOR
		base.OnValidate();
		#endif
	}
	#endif
}
