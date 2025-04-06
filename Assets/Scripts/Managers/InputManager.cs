using System;
using System.Collections.Generic;
using StrategyGameDemo.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StrategyGameDemo.Managers
{
	public class InputManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		
		[Header("Camera Drag Control")]
		private Camera mainCamera;
		private bool isDragging = false;
		private Vector3 initialCameraPos;
		private Vector2 initialMousePos;

		public static Action<Vector3> OnRightClick;
		public static Action<IDamageable> OnRightClickUnit;
		public static Action<Vector3> OnLeftClick;

		private void Start()
		{
			mainCamera = gameManager.GetMainCamera();
		}

		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (IsMouseOverUI()) return;
				
				isDragging = true;
				initialMousePos = Input.mousePosition;
				initialCameraPos = mainCamera.transform.position;
				
				RaycastHit2D hitInfo = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hitInfo && hitInfo.collider.TryGetComponent(out ISelectBehaviour clickable))
				{
					clickable.LeftClick();
				}
			}
			
			if (Input.GetMouseButtonUp(0))
			{
				isDragging = false;
				
				if (initialMousePos.Equals(Input.mousePosition))
					OnLeftClick?.Invoke(mainCamera.ScreenToWorldPoint(Input.mousePosition));
			}

			if (Input.GetMouseButtonDown(1))
			{
				RaycastHit2D hitInfo = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hitInfo && hitInfo.collider.TryGetComponent(out IDamageable damageable))
				{
					OnRightClickUnit?.Invoke(damageable);
				}
				else
				{
					OnRightClick?.Invoke(mainCamera.ScreenToWorldPoint(Input.mousePosition));
				}
			}

			CameraDrag();
		}

		public Vector3 GetMousePosition()
		{
			return mainCamera.ScreenToWorldPoint(Input.mousePosition);
		}

		private void CameraDrag()
		{
			if (isDragging && mainCamera != null)
			{
				Vector2 currentMousePos = Input.mousePosition;
				Vector2 delta = currentMousePos - initialMousePos;

				float scalingFactor = 2 * mainCamera.orthographicSize / Screen.height;
				float worldDeltaX = delta.x * scalingFactor;
				float worldDeltaY = delta.y * scalingFactor;

				Vector3 worldDelta = new Vector3(worldDeltaX, worldDeltaY, 0);
				mainCamera.transform.position = initialCameraPos - worldDelta;
			}
		}
		
		private bool IsMouseOverUI()
		{
			return EventSystem.current.IsPointerOverGameObject();
		}
	}
}