using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FloraData : UpdatableData
{
	public List<TreeModel> treeModels;
	public List<HerbModel> herbModels;

	public float treeRadius = 50;
	public float treeSpawnHeightThreshold = 100;
	public float treeScale = 6;

	[Range(0, 100)]
	public int herbSpawnProbability = 50;

	private int cumulativeTreeWeight = -1;
	private int cumulativeHerbWeight = -1;
	// Reduce spawn rate of both trees and herbs
	public void ReduceSpawnRate(float reductionFactor)
	{
		AdjustTreeWeights(reductionFactor);
		AdjustHerbWeights(reductionFactor);
	}

	private void AdjustTreeWeights(float reductionFactor)
	{
		foreach (var model in treeModels)
		{
			model.spawnWeight = Mathf.Max(1, (int)(model.spawnWeight * reductionFactor));
		}
		cumulativeTreeWeight = -1; // Reset so it recalculates
	}

	private void AdjustHerbWeights(float reductionFactor)
	{
		foreach (var model in herbModels)
		{
			model.spawnWeight = Mathf.Max(1, (int)(model.spawnWeight * reductionFactor));
		}
		cumulativeHerbWeight = -1; // Reset so it recalculates
	}

	private int SumTreeWeights()
	{
		int sum = 0;

		foreach (TreeModel model in treeModels)
		{
			sum += model.spawnWeight;
		}

		return sum;
	}

	private int SumHerbWeights()
	{
		int sum = 0;

		foreach (HerbModel model in herbModels)
		{
			sum += model.spawnWeight;
		}

		return sum;
	}

	public GameObject GetRandomModel()
	{
		if (cumulativeTreeWeight < 0)
		{
			cumulativeTreeWeight = SumTreeWeights();
		}

		if (cumulativeHerbWeight < 0)
		{
			cumulativeHerbWeight = SumHerbWeights();
		}

		// Generate random number
		System.Random random = new System.Random();

		//Pick between herb and tree placement
		int randNum = random.Next(0, 100);

		if (randNum <= herbSpawnProbability)
		{
			//Spawn herb
			randNum = random.Next(1, cumulativeHerbWeight);
			GameObject newHerb = RandomPicker(herbModels, randNum, cumulativeHerbWeight);
			newHerb.tag = "Herb";
			return newHerb;
		}
		else
		{
			//Spawn tree
			randNum = random.Next(1, cumulativeTreeWeight);
			GameObject newTree = RandomPicker(treeModels, randNum, cumulativeTreeWeight);
			newTree.tag = "Tree";
			return newTree;
		}
	}

	GameObject RandomPicker(List<TreeModel> floraList, int randNum, int cumulativeWeight)
	{
		while (randNum > 0)
		{
			foreach (TreeModel flora in floraList)
			{
				randNum -= flora.spawnWeight;

				if (randNum <= 0)
				{
					return flora.model;
				}
			}
		}

		return null;
	}

	GameObject RandomPicker(List<HerbModel> floraList, int randNum, int cumulativeWeight)
	{
		while (randNum > 0)
		{
			foreach (HerbModel flora in floraList)
			{
				randNum -= flora.spawnWeight;

				if (randNum <= 0)
				{
					return flora.model;
				}
			}
		}

		return null;
	}

	public class FloraModel
	{
		public int spawnWeight;
		public GameObject model;
	}

	[System.Serializable]
	public class TreeModel : FloraModel { }

	[System.Serializable]
	public class HerbModel : FloraModel
	{
		public float minHeightThreshold;
		public float maxHeightThreshold;
	}
}