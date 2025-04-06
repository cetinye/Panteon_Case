using System;
using System.Collections;
using StrategyGameDemo.Managers;
using UnityEngine;

namespace StrategyGameDemo
{
	public class PathFollow : MonoBehaviour
	{
		private float movementSpeed;
		private float rotationSpeed;
		
		private Vector2[] path;
		private int targetIndex;

		private IEnumerator followPathRoutine;

		public void SetValues(float movementSpeed, float rotationSpeed)
		{
			this.movementSpeed = movementSpeed;
			this.rotationSpeed = rotationSpeed;
		}

		public void SetDestination(Vector3 targetPosition)
		{
			PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
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
				
				transform.position = Vector2.MoveTowards(transform.position, current, movementSpeed * Time.deltaTime);
				LookAhead(current);
				
				yield return null;
			}
		}

		private void LookAhead(Vector2 targetAhead)
		{
			Vector2 direction = (targetAhead - (Vector2)transform.position).normalized;
			if (direction != Vector2.zero)
			{
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				float currentAngle = transform.eulerAngles.z;

				float smoothedAngle = Mathf.LerpAngle(currentAngle, angle, rotationSpeed * Time.deltaTime);
				transform.rotation = Quaternion.Euler(0f, 0f, smoothedAngle);
			}
		}
	}
}