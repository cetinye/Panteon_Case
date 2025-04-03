using UnityEngine;
using System;

namespace StrategyGameDemo.Models
{
	public abstract class UnitModel
	{
		public abstract UnitTypes UnitType { get; }
		
		[Header("Visual")]
		public Sprite UnitSprite;
		
		[Header("Stat Variables")]
		public float Health;
		public float AttackDamage;
		public float Range;
		public float MovementSpeed;
		public float RotationSpeed;

		public event Action<float> OnHealthChanged;

		public bool IsSelected;
		
		public virtual void InitializeFromData(Data.UnitSO data)
		{
			UnitSprite = data.UnitSprite;
			Health = data.Health;
			AttackDamage = data.AttackDamage;
			Range = data.Range;
			MovementSpeed = data.MovementSpeed;
			RotationSpeed = data.RotationSpeed;
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

	public enum UnitTypes
	{
		None,
		Soldier,
		Spearman,
		Archer
	}
}