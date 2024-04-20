using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class FloraPlacement
{

	public static List<Vector3> GeneratePoints(FloraData floraData, MeshCollider mesh, Vector2 chunkPosition, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30)
	{
		float cellSize = floraData.treeRadius / Mathf.Sqrt(2);
		sampleRegionSize /= 2;

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
				Vector2 candidate = spawnCentre + dir * Random.Range(floraData.treeRadius, 2 * floraData.treeRadius);
				if (IsValid(candidate, sampleRegionSize, cellSize, floraData.treeRadius, points, grid))
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

		                	//Offset the the point to fit to chunk  //Offset to chunk position
			offsetPosition.x = (point.x - (sampleRegionSize.x/2)) + chunkPosition.x;
			offsetPosition.z = (point.y - (sampleRegionSize.x/2)) + chunkPosition.y;
			offsetPosition.y = 200f;

			
			Ray ray = new Ray(offsetPosition, Vector3.down);
			if (mesh.Raycast(ray, out hit, 500f))
			{
				offsetPosition = hit.point;
			}
			
			//Ensure it hit a mesh
			//if (offsetPosition.y == 200f) continue;
			
			offsetPoints.Add(offsetPosition);
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