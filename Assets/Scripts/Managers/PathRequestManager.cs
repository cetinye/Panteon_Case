using System.Collections.Generic;
using UnityEngine;
using System;

namespace StrategyGameDemo.Managers
{
	public class PathRequestManager : MonoBehaviour
	{
		public static PathRequestManager Instance;
		
		Queue<PathRequest> requests = new Queue<PathRequest>();
		PathRequest current;
		
		Pathfinding pathfinding;
		private bool isProcessing;
		
		struct PathRequest
		{
			public Vector2 PathStart;
			public Vector2 PathEnd;
			public Action<Vector2[], bool> Callback;

			public PathRequest(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
			{
				 PathStart = pathStart;
				 PathEnd = pathEnd;
				 Callback = callback;
			}
		}

		private void Awake()
		{
			if (Instance != null && Instance != this) 
			{ 
				Destroy(this); 
			} 
			else 
			{ 
				Instance = this; 
			}
			
			pathfinding = GetComponent<Pathfinding>();
		}

		public static void RequestPath(Vector2 start, Vector2 end, Action<Vector2[], bool> callback)
		{
			PathRequest newRequest = new PathRequest(start, end, callback);
			Instance.requests.Enqueue(newRequest);
			Instance.TryProcessNext();
		}

		public void FinishedProcessing(Vector2[] path, bool success)
		{
			current.Callback?.Invoke(path, success);
			isProcessing = false;
			TryProcessNext();
		}

		private void TryProcessNext()
		{
			if (!isProcessing && requests.Count > 0)
			{
				current = requests.Dequeue();
				isProcessing = true;
				pathfinding.StartFindPath(current.PathStart, current.PathEnd);
			}
		}
	}
}