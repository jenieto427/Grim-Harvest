using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TreeData : UpdatableData
{
	public List<TreeModel> treeModels;

	public float treeRadius;

	public float treeScale;

	public float spawnHeightThreshold;

	public GameObject GetRandomModel()
	{
		System.Random rand = new System.Random();

		int randint = rand.Next(0, 100);
		
		foreach (TreeModel tree in treeModels)
		{
			if (tree.spawnWeight >= randint) return tree.model;
		}

		return treeModels[treeModels.Count - 1].model;
	}

	[System.Serializable]
	public class TreeModel
	{
		public int spawnWeight;
		public GameObject model;
	}
}