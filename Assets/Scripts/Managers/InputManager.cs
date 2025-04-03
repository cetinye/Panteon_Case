using StrategyGameDemo.Interfaces;
using UnityEngine;

namespace StrategyGameDemo.Managers
{
	public class InputManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		private Camera mainCamera;
		private ISelectBehaviour selectedUnit;

		private void Start()
		{
			mainCamera = gameManager.GetMainCamera();
		}

		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit2D hitInfo = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hitInfo && hitInfo.collider.TryGetComponent(out ISelectBehaviour clickable))
				{
					selectedUnit = clickable;
					clickable.LeftClick();
				}
			}

			if (Input.GetMouseButtonDown(1))
			{
				if (selectedUnit != null)
					selectedUnit.RightClick(mainCamera.ScreenToWorldPoint(Input.mousePosition));
			}
		}
	}
}