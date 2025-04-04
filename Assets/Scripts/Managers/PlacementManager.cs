using System;
using StrategyGameDemo.Factory;
using StrategyGameDemo.Models;
using UnityEngine;

namespace StrategyGameDemo.Managers
{
	public class PlacementManager : MonoBehaviour
	{
		public static PlacementManager Instance;
		
		[SerializeField] private GridController gridController;
		[SerializeField] private InputManager inputManager;
		
		public static Action<Vector3, Vector2> OnBuildingPlace;
		[SerializeField] private Vector3 previewOffset;
		private Vector2 lastPosition = Vector3.zero;
		
		private BuildingModel previewModel;
		private GameObject previewObject;
		private SpriteRenderer previewRenderer;
		private BuildingTypes selectedType = BuildingTypes.None;
		private bool isPlaceable = false;
		
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
		}

		private void Update()
		{
			if (selectedType == BuildingTypes.None) return;
			
			var mousePosition = inputManager.GetMousePosition();
			var node = gridController.GetNode(mousePosition);
			var gridPosition = new Vector2(node.GridX, node.GridY);
			
			if (lastPosition == gridPosition) return;

			isPlaceable = gridController.IsPlaceable(node, previewModel.BuildingSize);
			MovePreview(node.WorldPosition);
			ApplyFeedback(isPlaceable);
			lastPosition = gridPosition;
		}

		public void PreviewBuilding(BuildingTypes selectionType)
		{
			StopPreview();
			
			previewModel = BuildingFactory.GetBuilding(selectionType);
			previewObject = new GameObject("Preview", typeof(SpriteRenderer));
			previewRenderer = previewObject.GetComponent<SpriteRenderer>();
			previewRenderer.sprite = previewModel.BuildingSprite;
			previewObject.transform.localScale =
				new Vector3(previewModel.BuildingSize.x - 1, previewModel.BuildingSize.y - 1, 1);
			selectedType = previewModel.BuildingType;

			InputManager.OnLeftClick += OnLeftClicked;
		}

		private void StopPreview()
		{
			if (previewObject != null)
				Destroy(previewObject);
			
			selectedType = BuildingTypes.None;
		}

		private void OnLeftClicked(Vector3 pos)
		{
			PlaceBuilding(pos, previewModel);
			StopPreview();
			isPlaceable = false;
		}
		
		public void PlaceBuilding(Vector3 position, BuildingModel buildingModel)
		{
			if (!isPlaceable) return;
			
			OnBuildingPlace?.Invoke(position, buildingModel.BuildingSize);
		}

		private void MovePreview(Vector3 position)
		{
			previewObject.transform.position = position + previewOffset;
		}

		private void ApplyFeedback(bool isPlaceable)
		{
			var feedbackColor = isPlaceable ? Color.green : Color.red;
			feedbackColor.a = 0.5f;
			previewRenderer.color = feedbackColor;
		}
	}
}