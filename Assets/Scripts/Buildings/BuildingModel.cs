using System;
using UnityEngine;

namespace StrategyGameDemo.Models
{
	public abstract class BuildingModel
	{
		public abstract BuildingTypes BuildingType { get; }
		
		[Header("Visual")]
		public Sprite BuildingSprite;
		
		[Header("Stat Variables")]
		public string BuildingName;
		public float Health;
		public Vector2 BuildingSize;

		public event Action<float> OnHealthChanged;
		
		public virtual void InitializeFromData(Data.BuildingSO data)
		{
			BuildingName = data.BuildingName;
			BuildingSprite = data.BuildingSprite;
			Health = data.Health;
			BuildingSize = data.BuildingSize;
		}
		
		public virtual void TakeDamage(float damage)
		{
			Health -= damage;
			Health = Mathf.Max(Health, 0);
			
			OnHealthChanged?.Invoke(Health);

			if (Health <= 0)
			{
				Die();
			}
		}

		protected virtual void Die() { }
	}

	public enum BuildingTypes
	{
		None,
		Barrack,
		PowerPlant
	}
}