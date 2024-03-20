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

}
