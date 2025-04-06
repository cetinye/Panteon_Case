using System;
using UnityEngine;

namespace StrategyGameDemo
{
	public class Node : IHeapItem<Node>
	{
		public bool IsWalkable;
		public Vector2 WorldPosition;
		public int GridX;
		public int GridY;

		public int GCost;
		public int HCost;
		public int FCost => GCost + HCost;

		public Node Parent;
		
		private int heapIndex;

		public Node(bool isWalkable, Vector2 worldPosition, int gridX, int gridY)
		{
			IsWalkable = isWalkable;
			WorldPosition = worldPosition;
			GridX = gridX;
			GridY = gridY;
		}

		public int CompareTo(Node other)
		{
			int compare = FCost.CompareTo(other.FCost);
			if (compare == 0)
			{
				compare = HCost.CompareTo(other.HCost);
			}

			return -compare; //- for returning 1 if lower
		}

		public int HeapIndex
		{
			get => heapIndex;
			set => heapIndex = value;
		}
	}
}