using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyGameDemo
{
	public class PathFollow : MonoBehaviour
	{
		public Transform target;
		
		[SerializeField] private float speed;
		
		private Vector2[] path;
		private int targetIndex;

		private IEnumerator followPathRoutine;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		}

		private void OnDrawGizmos()
		{
			if (path == null) return;

			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);
				
				if (i == targetIndex)
					Gizmos.DrawLine(transform.position, path[i]);
				else
					Gizmos.DrawLine(path[i-1], path[i]);
			}
		}

		public void OnPathFound(Vector2[] path, bool pathSuccessful)
		{
			if (pathSuccessful && path.Length > 0)
			{
				this.path = path;

				if (followPathRoutine != null)
				{
					StopCoroutine(followPathRoutine);
					followPathRoutine = null;
					targetIndex = 0;
				}
				
				followPathRoutine = FollowPath();
				StartCoroutine(followPathRoutine);
			}
		}

		IEnumerator FollowPath()
		{
			Vector2 current = path[0];

			while (true)
			{
				if (Mathf.Approximately(transform.position.x, current.x) &&
				    Mathf.Approximately(transform.position.y, current.y))
				{
					targetIndex++;
					if (targetIndex >= path.Length)
					{
						targetIndex = 0;
						path = Array.Empty<Vector2>();
						yield break;
					}
					current = path[targetIndex];
				}
				
				transform.position = Vector2.MoveTowards(transform.position, current, speed * Time.deltaTime);
				yield return null;
			}
		}
	}
}