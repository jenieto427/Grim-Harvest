using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

/* 
 * CODE BASED ON IMPLEMENTATION BY SEBASTIAN LAGUE
 * FROM HIS PLAYLIST ON YOUTUBE "PROCEDURAL TERRAIN GENERATION"
 */

public class EndlessTerrain : MonoBehaviour
{

	const float viewerMoveThresholdForChunkUpdate = 25f;
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

	public LODInfo[] detailLevels;
	public static float maxViewDst;

	static int chunkNumber = 0;

	public Transform viewer;
	public Material mapMaterial;

	public FloraData floraData;
	public GameObject treeParent;
	public GameObject plantParent;

	public static Vector2 viewerPosition;
	Vector2 viewerPositionOld;
	static MapGenerator mapGenerator;
	static int chunkSize;
	int chunksVisibleInViewDst;

	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

	public void Start()
	{
		mapGenerator = FindObjectOfType<MapGenerator>();

		maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
		chunkSize = mapGenerator.mapChunkSize - 1;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

		UpdateVisibleChunks();
	}


	void Update()
	{
		viewerPosition = new Vector2(viewer.position.x, viewer.position.z) / mapGenerator.terrainData.uniformScale;

		if ((viewerPositionOld-viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
		{
			viewerPositionOld = viewerPosition;
			UpdateVisibleChunks();
		}
	}

	public void ClearTerrain() {
		terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
		terrainChunksVisibleLastUpdate = new List<TerrainChunk>();
	}

	void UpdateVisibleChunks()
	{

		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
		{
			terrainChunksVisibleLastUpdate[i].SetVisible(false);
		}
		terrainChunksVisibleLastUpdate.Clear();

		int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
				{
					terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
				}
				else
				{
					terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial, floraData, treeParent, plantParent));
				}

			}
		}
	}

	public class TerrainChunk
	{

		GameObject meshObject;
		Vector2 position;
		Bounds bounds;

		MeshRenderer meshRenderer;
		MeshFilter meshFilter;
		MeshCollider meshCollider;
		//NavMeshSurface navMeshSurface;

		LODInfo[] detailLevels;
		LODMesh[] lodMeshes;
		LODMesh collisionLODMesh;

		FloraData floraData;
		GameObject treeParent;
		GameObject plantParent;
		List<GameObject> flora = new List<GameObject>();
		bool treesGenerated = false;

		MapData mapData;
		bool mapDataRecieved;
		int previousLODIndex = -1;

		public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material, FloraData floraData, GameObject treeParent, GameObject plantParent)
		{
			this.floraData = floraData;
			this.treeParent = treeParent;
			this.plantParent = plantParent;
			this.detailLevels = detailLevels;

			position = coord * size;

			bounds = new Bounds(position, Vector2.one * size);
			Vector3 positionV3 = new Vector3(position.x, 0, position.y);

			meshObject = new GameObject("Terrain Chunk " + chunkNumber++);
			meshObject.layer = LayerMask.NameToLayer("Ground");

			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			meshFilter = meshObject.AddComponent<MeshFilter>();
			meshCollider = meshObject.AddComponent<MeshCollider>();

			meshRenderer.material = material;

			meshObject.transform.position = positionV3 * mapGenerator.terrainData.uniformScale;
			meshObject.transform.parent = parent;
			meshObject.transform.localScale = Vector3.one * mapGenerator.terrainData.uniformScale;
			SetVisible(false);

			lodMeshes = new LODMesh[detailLevels.Length];
			for (int i = 0; i < detailLevels.Length; i++)
			{
				lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
				if (detailLevels[i].useForCollider)
				{
					collisionLODMesh = lodMeshes[i];
				}
			}

			mapGenerator.RequestMapData(position, OnMapDataReceived);
			this.floraData = floraData;
		}

		void OnMapDataReceived(MapData mapData)
		{
			this.mapData = mapData;
			mapDataRecieved = true;

			UpdateTerrainChunk();
		}

		public void UpdateTerrainChunk()
		{
			if (mapDataRecieved)
			{
				float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
				bool visible = viewerDstFromNearestEdge <= maxViewDst;

				if (visible)
				{
					int lodIndex = 0;

					for (int i = 0; i < detailLevels.Length - 1; i++)
					{
						if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold)
						{
							lodIndex = i + 1;
						}
						else
						{
							break;
						}
					}

					if (lodIndex != previousLODIndex)
					{
						LODMesh lodMesh = lodMeshes[lodIndex];
						if (lodMesh.hasMesh)
						{
							previousLODIndex = lodIndex;
							meshFilter.mesh = lodMesh.mesh;
						}

						else if (!lodMesh.hasRequestedMesh)
						{
							lodMesh.RequestMesh(mapData);
						}
					}

					if (lodIndex == 0)
					{
						if (collisionLODMesh.hasMesh)
						{
							meshCollider.sharedMesh = collisionLODMesh.mesh;
						} else if (!collisionLODMesh.hasRequestedMesh)
						{
							collisionLODMesh.RequestMesh(mapData);
						}
					}

					//Generate tree positions
					if (!treesGenerated && collisionLODMesh.hasMesh)
					{
						List<Vector3> floraPoints = FloraPlacement.GeneratePoints(floraData, meshCollider, chunkSize, position, Vector2.one * (chunkSize * 10));

						System.Random rand = new System.Random();
						foreach (Vector3 treePos in floraPoints)
						{
							//Instantiate new flora
							GameObject newFlora = Instantiate(floraData.GetRandomModel(), treePos, Quaternion.identity);

							//Set parent
							newFlora.transform.SetParent(newFlora.CompareTag("Tree") ? treeParent.transform : plantParent.transform);

							//Change scale
							newFlora.transform.localScale = Vector3.one * floraData.treeScale;

							//Add to flora list
							flora.Add(newFlora);
						}

						treesGenerated = true;
					}
					terrainChunksVisibleLastUpdate.Add(this);
				}

				SetVisible(visible);
			}
		}

		public void SetVisible(bool visible)
		{
			if (meshObject == null) return;

			meshObject.SetActive(visible);

			foreach (var tree in flora)
			{
				tree.SetActive(visible);
			}
		}

		public bool IsVisible()
		{
			return meshObject.activeSelf;
		}
/*
		public void CreateNavMesh()
		{
			navMeshSurface = meshObject.AddComponent<NavMeshSurface>();

			// Set up NavMeshSurface parameters
			navMeshSurface.agentTypeID = NavMesh.GetSettingsByIndex(0).agentTypeID; // Set the default agent type
			navMeshSurface.layerMask = LayerMask.GetMask(mainMapLayer);
			navMeshSurface.collectObjects = CollectObjects.All; // Collect all objects within the specified layer mask

			// Generate NavMesh at runtime
			navMeshSurface.BuildNavMesh();
		}
*/
	}

	class LODMesh
	{
		public Mesh mesh;
		public bool hasRequestedMesh;
		public bool hasMesh;
		int lod;
		System.Action updateCallBack;

		public LODMesh(int lod, System.Action updateCallBack)
		{
			this.lod = lod;
			this.updateCallBack = updateCallBack;
		}

		void OnMeshDataRecieved(MeshData meshData)
		{
			mesh = meshData.CreateMesh();
			hasMesh = true;

			updateCallBack();
		}

		public void RequestMesh(MapData mapData)
		{
			hasRequestedMesh = true;
			mapGenerator.RequestMeshData(mapData, lod, OnMeshDataRecieved);
		}
	}

	[System.Serializable]
	public struct LODInfo
	{
		public int lod;
		public float visibleDstThreshold;
		public bool useForCollider;
	}
}