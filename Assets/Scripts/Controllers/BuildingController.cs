using System;
using StrategyGameDemo.Factory;
using StrategyGameDemo.Interfaces;
using StrategyGameDemo.Models;
using StrategyGameDemo.Views;
using UnityEngine;

namespace StrategyGameDemo
{
	public class BuildingController : MonoBehaviour, ISelectBehaviour
	{
		[SerializeField] private BuildingTypes buildingType;
		
		private BuildingModel model;
		private BuildingView view;
		
		private BoxCollider2D boxCollider;
		
		private void OnDisable()
		{
			model.OnHealthChanged -= UpdateHealth;
		}

		public void Initialize()
		{
			model = BuildingFactory.GetBuilding(buildingType);
			
			view = GetComponent<BuildingView>();
			view.SetBuildingSprite(model.BuildingSprite);
			view.SetBuildingSpriteSize(model.BuildingSize);
			view.UpdateHealthText(model.Health);
			
			boxCollider = GetComponent<BoxCollider2D>();
			SetColliderSize();

			model.OnHealthChanged += UpdateHealth;
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
			view.ShowBuildingInfo(model);
		}

		public void RightClick(Vector3 position)
		{
			
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