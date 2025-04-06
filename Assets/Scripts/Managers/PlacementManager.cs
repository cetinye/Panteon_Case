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
		
		public static Action<Vector3, BuildingModel, BuildingController> OnBuildingPlace;
		[SerializeField] private Vector3 previewOffset;
		private Vector2 lastPosition = Vector3.zero;
		
		private BuildingModel previewModel;
		private GameObject previewObject;
		private SpriteRenderer previewRenderer;
		private BuildingController previewBuildingController;
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
			previewObject = ConcreteBuildingFactory.CreateBuildingInstance(selectionType, transform.position, transform.rotation, transform, null, true);
			previewBuildingController = previewObject.GetComponent<BuildingController>();
			previewRenderer = previewBuildingController.GetRenderer();
			selectedType = selectionType;

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
			Node placedNode = GridController.Instance.GetNode(pos);
			PlaceBuilding(placedNode.WorldPosition, placedNode,previewModel);
			StopPreview();
			isPlaceable = false;
		}
		
		public void PlaceBuilding(Vector3 position, Node placedNode, BuildingModel buildingModel)
		{
			if (!isPlaceable) return;
			
			var spawnedBuilding = ConcreteBuildingFactory.CreateBuildingInstance(buildingModel.BuildingType, position, transform.rotation, null, placedNode);
			var buildingController = spawnedBuilding.GetComponent<BuildingController>();
			
			OnBuildingPlace?.Invoke(position, buildingModel, buildingController);
		}

		private void MovePreview(Vector3 position)
		{
			previewObject.transform.position = position + previewOffset;
		}

		private void ApplyFeedback(bool isPlaceable)
		{
			var feedbackColor = isPlaceable ? Color.green : Color.red;
			feedbackColor.a = 0.5f;
			previewBuildingController.SetRendererColor(feedbackColor);
		}
	}
}