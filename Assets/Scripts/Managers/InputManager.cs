using StrategyGameDemo.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StrategyGameDemo.Managers
{
	public class InputManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		private Camera mainCamera;
		private ISelectBehaviour selectedUnit;
		
		[Header("Camera Drag Control")]
		private bool isDragging = false;
		private Vector3 initialCameraPos;
		private Vector2 initialMousePos;

		private void Start()
		{
			mainCamera = gameManager.GetMainCamera();
		}

		void Update()
		{
			if (IsMouseOverUI()) return;
			
			if (Input.GetMouseButtonDown(0))
			{
				isDragging = true;
				initialMousePos = Input.mousePosition;
				initialCameraPos = mainCamera.transform.position;
				
				RaycastHit2D hitInfo = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hitInfo && hitInfo.collider.TryGetComponent(out ISelectBehaviour clickable))
				{
					selectedUnit = clickable;
					clickable.LeftClick();
				}
			}
			
			if (Input.GetMouseButtonUp(0))
			{
				isDragging = false;
			}

			if (Input.GetMouseButtonDown(1))
			{
				if (selectedUnit != null)
					selectedUnit.RightClick(mainCamera.ScreenToWorldPoint(Input.mousePosition));
			}

			CameraDrag();
		}

		private void CameraDrag()
		{
			if (isDragging && mainCamera != null && !IsMouseOverUI())
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