using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TreeData : UpdatableData
{
	public List<TreeModel> treeModels;
	private int cumulativeWeight = -1;

	public float treeRadius;

	public float treeScale;

	public float spawnHeightThreshold;

	private int SumWeights()
	{
		int sum = 0;

		foreach (TreeModel model in treeModels)
		{
			Debug.Log(model.model);
			sum += model.spawnWeight;
		}

		Debug.Log(sum);

		return sum;
	}

	public GameObject GetRandomModel()
	{
		if (cumulativeWeight < 0)
		{
			cumulativeWeight = SumWeights();
		}

		// Generate random number
		System.Random random = new System.Random();
		int randNum = random.Next(1, cumulativeWeight);

		while(randNum > 0)
		{
			foreach (TreeModel tree in treeModels)
			{
				randNum -= tree.spawnWeight;

				if (randNum <= 0)
				{
					return tree.model;
				}
			}
		}

		return null;
	}

	[System.Serializable]
	public class TreeModel
	{
		public int spawnWeight;
		public GameObject model;
	}
}