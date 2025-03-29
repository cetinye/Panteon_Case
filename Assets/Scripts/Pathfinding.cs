using System.Collections.Generic;
using UnityEngine;

namespace StrategyGameDemo
{
	public class Pathfinding : MonoBehaviour
	{
		public Transform seeker;
		public Transform target;
		
		private const int DIAGONAL_COST = 14;
		private const int STRAIGHT_COST = 10;
		
		private GridController gridController;
		
		private Heap<Node> openSet;
		private HashSet<Node> closedSet = new HashSet<Node>();

		private void Start()
		{
			gridController = GetComponent<GridController>();

			Initialize();
		}

		private void Initialize()
		{
			openSet = new Heap<Node>(gridController.MaxSize);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				FindPath(seeker.position, target.position);
		}

		private void FindPath(Vector2 startPos, Vector2 endPos)
		{
			Node startNode = gridController.GetNode(startPos);
			Node endNode = gridController.GetNode(endPos);

			openSet.Clear();
			closedSet.Clear();
			
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == endNode)
				{
					RetracePath(startNode, endNode);
					return;
				}

				foreach (var neighbour in gridController.GetNeighbours(currentNode))
				{
					if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
						continue;
					
					int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
					{
						neighbour.GCost = newMovementCostToNeighbour;
						neighbour.HCost = GetDistance(neighbour, endNode);
						neighbour.Parent = currentNode;
						
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
				
			}
		}

		private void RetracePath(Node startNode, Node endNode)
		{
			List<Node> path = new List<Node>();
			Node currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.Parent;
			}
			
			path.Reverse();
			
			gridController.Path = path;
		}

		private int GetDistance(Node start, Node end)
		{
			int distanceX = Mathf.Abs(start.GridX - end.GridX);
			int distanceY = Mathf.Abs(start.GridY - end.GridY);

			if (distanceX > distanceY)
			{
				return DIAGONAL_COST * distanceY + STRAIGHT_COST * (distanceX - distanceY);
			}
			
			return DIAGONAL_COST * distanceX + STRAIGHT_COST * (distanceY - distanceX);
		}
	}
}