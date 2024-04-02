using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class TreePlacement
{

	public static List<Vector3> GeneratePoints(TreeData treeData, MeshCollider mesh, int chunkSize, Vector2 chunkPosition, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30)
	{
		float cellSize = treeData.treeRadius / Mathf.Sqrt(2);

		int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
		List<Vector2> points = new List<Vector2>();
		List<Vector2> spawnPoints = new List<Vector2>();

		//Generate 2d points
		spawnPoints.Add(sampleRegionSize / 2);
		while (spawnPoints.Count > 0)
		{
			int spawnIndex = Random.Range(0, spawnPoints.Count);
			Vector2 spawnCentre = spawnPoints[spawnIndex];
			bool candidateAccepted = false;

			for (int i = 0; i < numSamplesBeforeRejection; i++)
			{
				float angle = Random.value * Mathf.PI * 2;
				Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
				Vector2 candidate = spawnCentre + dir * Random.Range(treeData.treeRadius, 2 * treeData.treeRadius);
				if (IsValid(candidate, sampleRegionSize, cellSize, treeData.treeRadius, points, grid))
				{
					points.Add(candidate);
					spawnPoints.Add(candidate);
					grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
					candidateAccepted = true;
					break;
				}
			}


			if (!candidateAccepted)
			{
				spawnPoints.RemoveAt(spawnIndex);
			}

		}

		//Cast 2d points to 3d and find height of each point
		List<Vector3> offsetPoints = new List<Vector3>();
		Vector3 offsetPosition = Vector2.zero;
		RaycastHit hit;
		foreach(Vector2 point in points)
		{
			offsetPosition = point;

			offsetPosition.x = point.x - (chunkSize * 5) + (chunkPosition.x * 10);
			offsetPosition.z = point.y - (chunkSize * 5) + (chunkPosition.y * 10);
			offsetPosition.y = 200;

			Ray ray = new Ray(offsetPosition, Vector3.down);
			if (mesh.Raycast(ray, out hit, 3.0f * 200))
			{
				offsetPosition = hit.point;
			}

			if (treeData.spawnHeightThreshold > offsetPosition.y)
			{
				offsetPoints.Add(offsetPosition);
			}
		}

		return offsetPoints;
	}

	static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
	{
		if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
		{
			int cellX = (int)(candidate.x / cellSize);
			int cellY = (int)(candidate.y / cellSize);
			int searchStartX = Mathf.Max(0, cellX - 2);
			int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
			int searchStartY = Mathf.Max(0, cellY - 2);
			int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

			for (int x = searchStartX; x <= searchEndX; x++)
			{
				for (int y = searchStartY; y <= searchEndY; y++)
				{
					int pointIndex = grid[x, y] - 1;
					if (pointIndex != -1)
					{
						float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
						if (sqrDst < radius * radius)
						{
							return false;
						}
					}
				}
			}
			return true;
		}
		return false;
	}
}