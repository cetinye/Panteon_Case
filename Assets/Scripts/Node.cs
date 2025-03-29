using UnityEngine;

namespace StrategyGameDemo
{
	public class Node
	{
		public bool IsWalkable;
		public Vector2 WorldPosition;
		public int GridX;
		public int GridY;

		public int GCost;
		public int HCost;
		public int FCost => GCost + HCost;

		public Node Parent;

		public Node(bool isWalkable, Vector2 worldPosition, int gridX, int gridY)
		{
			IsWalkable = isWalkable;
			WorldPosition = worldPosition;
			GridX = gridX;
			GridY = gridY;
		}
	}
}