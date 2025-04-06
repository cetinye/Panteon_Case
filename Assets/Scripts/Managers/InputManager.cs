using System;
using System.Collections.Generic;
using StrategyGameDemo.Controllers;
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
		private CameraController cameraController;
		private bool isLeftMouseButtonDown;
		private Vector2 dragStartMousePos;

		public static Action<Vector3> OnRightClick;
		public static Action<IDamageable> OnRightClickUnit;
		public static Action<Vector3> OnLeftClick;

		private void Start()
		{
			mainCamera = gameManager.GetMainCamera();
			cameraController = mainCamera.GetComponent<CameraController>();
		}

		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				dragStartMousePos = Input.mousePosition;
				
				if (IsMouseOverUI()) return;
				
				isLeftMouseButtonDown = true;
				
				cameraController.StartDragging(Input.mousePosition);
				
				RaycastHit2D hitInfo = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hitInfo && hitInfo.collider.TryGetComponent(out ISelectBehaviour clickable))
				{
					clickable.LeftClick();
				}
			}
			
			if (isLeftMouseButtonDown && cameraController != null)
			{
				cameraController.UpdateDrag(Input.mousePosition);
			}
			
			if (Input.GetMouseButtonUp(0))
			{
				if (!isLeftMouseButtonDown) return;
				isLeftMouseButtonDown = false;
				
				cameraController.StopDragging();
				
				float clickVsDragThreshold = 2f;
				if (Vector2.Distance(dragStartMousePos, Input.mousePosition) < clickVsDragThreshold)
				{
					if (!IsMouseOverUI())
					{
						OnLeftClick?.Invoke(mainCamera.ScreenToWorldPoint(Input.mousePosition));
					}
				}
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
		}

		public Vector3 GetMousePosition()
		{
			return mainCamera.ScreenToWorldPoint(Input.mousePosition);
		}
		
		private bool IsMouseOverUI()
		{
			return EventSystem.current.IsPointerOverGameObject();
		}
	}
}