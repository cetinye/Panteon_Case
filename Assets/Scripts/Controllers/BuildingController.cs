using System;
using System.Collections.Generic;
using StrategyGameDemo.Factory;
using StrategyGameDemo.Interfaces;
using StrategyGameDemo.Managers;
using StrategyGameDemo.Models;
using StrategyGameDemo.UI;
using StrategyGameDemo.Views;
using UnityEngine;

namespace StrategyGameDemo
{
	public class BuildingController : MonoBehaviour, ISelectBehaviour
	{
		[SerializeField] private BuildingTypes buildingType;
		[SerializeField] private List<Transform> spawnPoints = new List<Transform>();
		
		private BuildingModel model;
		private BuildingView view;
		
		private BoxCollider2D boxCollider;
		private bool isInitialized;
		private bool isPreview;

		private void OnDisable()
		{
			model.OnHealthChanged -= UpdateHealth;
		}

		public void Initialize(Node placedNode, bool isPreview = false)
		{
			if (isInitialized) return;
			
			isInitialized = true;
			this.isPreview = isPreview;
			
			model = BuildingFactory.GetBuilding(buildingType);
			model.SetPlacedNode(placedNode);
			
			view = GetComponent<BuildingView>();
			view.SetBuildingSprite(model.BuildingSprite);
			view.SetBuildingSpriteSize(model.BuildingSize);
			view.UpdateHealthText(model.Health);
			
			boxCollider = GetComponent<BoxCollider2D>();
			SetColliderSize();
			
			if (isPreview) return;
			
			model.OnHealthChanged += UpdateHealth;
			
			if (model.ProducableUnits.Count > 0)
			{
				SetupUnitSpawnPosition();
				UnitProductionInformation.OnUnitSpawnClick += OnUnitSpawnClicked;
			}
		}

		private void SetupUnitSpawnPosition()
		{
			Node placedNode = model.PlacedNode;
			Vector2 worldSize = GridController.Instance.GetWorldSize();
			Vector3 spawnPos = transform.TransformPoint(spawnPoints[1].localPosition);
			
			// 0-North, 1-South, 2-East, 3-West
			if (placedNode != null)
			{
				if (placedNode.GridY == 0)
					spawnPos = transform.TransformPoint(spawnPoints[0].localPosition);
				
				if (placedNode.GridY + (int)model.BuildingSize.y - 1 == (int)worldSize.y - 1)
					spawnPos = transform.TransformPoint(spawnPoints[1].localPosition);
				
				if (placedNode.GridX == 0)
					spawnPos = transform.TransformPoint(spawnPoints[2].localPosition);
				
				if (placedNode.GridX + (int)model.BuildingSize.x - 1 == (int)worldSize.x - 1)
					spawnPos = transform.TransformPoint(spawnPoints[3].localPosition);
			}
			
			model.SetUnitSpawnPosition(spawnPos);
		}

		private void SetColliderSize()
		{
			boxCollider.size = model.BuildingSize - new Vector2(0.1f, 0.1f);
		}
		
		public void TakeDamage(float damage)
		{
			model.TakeDamage(damage);
		}
		
		private void UpdateHealth(float health)
		{
			view.UpdateHealthText(health);
		}

		public void LeftClick()
		{
			if (isInitialized &&  !isPreview)
				view.ShowBuildingInfo(model);
		}

		public void RightClick(Vector3 position)
		{
			
		}
		
		private bool IsInformationPanelActive()
		{
			return model == UIManager.Instance.GetActiveBuildingModel();
		}

		private void OnUnitSpawnClicked(UnitModel unitModel)
		{
			if (IsInformationPanelActive())
			{
				var spawnedUnit =
					ConcreteUnitFactory.CreateUnitInstance(unitModel.UnitType, model.UnitSpawnPosition,
						Quaternion.identity);
			}
		}

		public SpriteRenderer GetRenderer()
		{
			return view.GetRenderer();
		}

		public void SetRendererColor(Color c)
		{
			view.SetSpriteColor(c);
		}
	}
}