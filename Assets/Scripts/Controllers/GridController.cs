using System.Collections.Generic;
using StrategyGameDemo.Managers;
using UnityEngine;

namespace StrategyGameDemo
{
	public class GridController : MonoBehaviour
	{
		[SerializeField] private Pathfinding pathfinding;
		[SerializeField] private Vector2 gridWorldSize;
		[SerializeField] private float nodeRadius;
		[SerializeField] private LayerMask unwalkableMask;

		[Header("DEBUG")]
		[SerializeField] private bool showDebug;
		
		private Node[,] grid;
		private float nodeDiameter;
		private int gridSizeX, gridSizeY;
		
		private MeshFilter meshFilter;
		private MeshRenderer meshRenderer;
		[SerializeField] private Material meshMaterial;
		
		public int MaxSize => gridSizeX * gridSizeY;

		#region Gizmos

		private void OnDrawGizmos()
		{
			if (!showDebug) return;
				
			DrawGridOutline();
			DrawGrid();
		}
		
		private void DrawGridOutline() => Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

		private void DrawGrid()
		{
			if (grid != null)
			{
				foreach (var n in grid)
				{
					Gizmos.color = n.IsWalkable ? new Color(1, 1, 1, 0.25f) : Color.red;
					Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter - 0.1f));
				}
			}
		}

		#endregion

		public void InitializeGrid()
		{
			nodeDiameter = nodeRadius * 2;
			gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
			gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
			
			pathfinding.Initialize();
			
			meshFilter = gameObject.AddComponent<MeshFilter>();
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.material = meshMaterial;

			PlacementManager.OnBuildingPlace += OnBuildingPlaced;
			
			SpawnGrid();
		}

		private void SpawnGrid()
		{
			grid = new Node[gridSizeX, gridSizeY];
			for (int y = 0; y < gridSizeY; y++)
			{
				for (int x = 0; x < gridSizeX; x++)
				{
					Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
					                     Vector3.up * (y * nodeDiameter + nodeRadius);
					bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask);
					grid[x, y] = new Node(walkable, worldPoint, x, y);
				}
			}

			UpdateGridMesh();
		}

		private void UpdateGridMesh()
		{
			Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();
			List<Color> colors = new List<Color>();

			for (int y = 0; y < gridSizeY; y++)
			{
				for (int x = 0; x < gridSizeX; x++)
				{
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);

					float halfSize = nodeRadius - 0.05f;
					int vertIndex = vertices.Count;
					vertices.Add(worldPoint + new Vector3(-halfSize, -halfSize, 0));
					vertices.Add(worldPoint + new Vector3(halfSize, -halfSize, 0));
					vertices.Add(worldPoint + new Vector3(halfSize, halfSize, 0));
					vertices.Add(worldPoint + new Vector3(-halfSize, halfSize, 0));

					triangles.Add(vertIndex);
					triangles.Add(vertIndex + 1);
					triangles.Add(vertIndex + 2);
					triangles.Add(vertIndex);
					triangles.Add(vertIndex + 2);
					triangles.Add(vertIndex + 3);

					Color color = grid[x, y].IsWalkable ? new Color(1, 1, 1, 0.25f) : Color.red;
					colors.Add(color);
					colors.Add(color);
					colors.Add(color);
					colors.Add(color);
				}
			}

			Mesh mesh = new Mesh();
			mesh.vertices = vertices.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.colors = colors.ToArray();
			meshFilter.mesh = mesh;
		}

		public Node GetNode(Vector2 worldPosition)
		{
			float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
			float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
			
			percentX = Mathf.Clamp01(percentX);
			percentY = Mathf.Clamp01(percentY);
			
			int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
			int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

			return grid[x, y];
		}

		public bool IsPlaceable(Vector2 worldPosition, Vector2 size)
		{
			Node n = GetNode(worldPosition);

			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					if (!grid[n.GridX + x, n.GridY + y].IsWalkable)
						return false;
				}
			}

			return true;
		}
		
		public bool IsPlaceable(Node node, Vector2 size)
		{
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					if (gridSizeX <= node.GridX + x || gridSizeY <= node.GridY + y) 
						return false;
					
					if (!grid[node.GridX + x, node.GridY + y].IsWalkable)
						return false;
				}
			}

			return true;
		}

		private void OnBuildingPlaced(Vector3 pos, Vector2 buildingSize)
		{
			Node node = GetNode(pos);
		
			for (int x = 0; x < buildingSize.x; x++)
			{
				for (int y = 0; y < buildingSize.y; y++)
				{
					grid[node.GridX + x, node.GridY + y].IsWalkable = false;
				}
			}

			UpdateGridMesh();
		}

		public List<Node> GetNeighbours(Node node)
		{
			List<Node> neighbours = new List<Node>();

			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if (x == 0 && y == 0)
						continue;
					
					int checkX = node.GridX + x;
					int checkY = node.GridY + y;

					if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
					{
						neighbours.Add(grid[checkX, checkY]);
					}
				}
			}

			return neighbours;
		}
	}
}