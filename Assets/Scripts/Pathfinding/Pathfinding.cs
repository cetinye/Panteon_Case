using System;
using System.Collections;
using System.Collections.Generic;
using StrategyGameDemo.Controllers;
using StrategyGameDemo.Managers;
using UnityEngine;

namespace StrategyGameDemo
{
	public class Pathfinding : MonoBehaviour
	{
		private const int DIAGONAL_COST = 14;
		private const int STRAIGHT_COST = 10;
		
		private GridController gridController;
		
		private Heap<Node> openSet;
		private HashSet<Node> closedSet = new HashSet<Node>();
		
		private PathRequestManager pathRequestManager;

		private void Awake()
		{
			gridController = GetComponent<GridController>();
			pathRequestManager = GetComponent<PathRequestManager>();
		}

		public void Initialize()
		{
			openSet = new Heap<Node>(gridController.MaxSize);
		}

		public void StartFindPath(Vector2 startPos, Vector2 targetPos)
		{
			StartCoroutine(FindPath(startPos, targetPos));
		}

		IEnumerator FindPath(Vector2 startPos, Vector2 endPos)
		{
			Vector2[] waypoints = new Vector2[0];
			bool pathFound = false;
			
			Node startNode = gridController.GetNode(startPos);
			Node endNode = gridController.GetNode(endPos);

			if (!endNode.IsWalkable)
			{
				pathRequestManager.FinishedProcessing(Array.Empty<Vector2>(), false);
				yield break;
			}
			
			openSet.Clear();
			closedSet.Clear();
			
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == endNode)
				{
					pathFound = true;
					break;
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
						else
							openSet.UpdateItem(neighbour);
					}
				}
				
			}

			yield return null;

			if (pathFound)
			{
				waypoints = RetracePath(startNode, endNode);
			}
			pathRequestManager.FinishedProcessing(waypoints, pathFound);
		}

		private Vector2[] RetracePath(Node startNode, Node endNode)
		{
			List<Node> path = new List<Node>();
			Node currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.Parent;
			}

			Vector2[] waypoints = SimplifyPath(path);
			Array.Reverse(waypoints);
			return waypoints;
		}

		private Vector2[] SimplifyPath(List<Node> path)
		{
			List<Vector2> waypoints = new List<Vector2>();
			Vector2 directionOld = Vector2.zero;

			for (int i = 1; i < path.Count; i++)
			{
				Vector2 directionNew = new Vector2(path[i-1].GridX - path[i].GridX, path[i-1].GridY - path[i].GridY);
				if (directionNew != directionOld)
				{
					waypoints.Add(path[i-1].WorldPosition);
				}
				directionOld = directionNew;
			}

			if (path.Count > 0)
				waypoints.Add(path[^1].WorldPosition);

			return waypoints.ToArray();
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