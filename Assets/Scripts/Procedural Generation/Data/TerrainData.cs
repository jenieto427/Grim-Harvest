using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : UpdatableData
{
	//Changes scale of main map
	public float uniformScale = 5f;


	[Range(0f, 25f)]
	public float meshHeightMultiplier;

	public AnimationCurve meshHeightCurve;

	public bool useFlatShading;

	public float minHeight
	{
		get
		{
			return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
		}
	}

	public float maxHeight
	{
		get
		{
			return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(1);
		}
	}
}
