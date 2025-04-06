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
		public string UnitName;
		public float Health;
		public float AttackDamage;
		public float Range;
		public float MovementSpeed;
		public float RotationSpeed;

		public event Action<float> OnHealthChanged;

		public bool IsSelected;
		
		public virtual void InitializeFromData(Data.UnitSO data)
		{
			UnitName = data.UnitName;
			UnitSprite = data.UnitSprite;
			Health = data.Health;
			AttackDamage = data.AttackDamage;
			Range = data.AttackRange;
			MovementSpeed = data.MovementSpeed;
			RotationSpeed = data.RotationSpeed;
		}

		public virtual void TakeDamage(float damage)
		{
			Health -= damage;
			Health = Mathf.Max(Health, 0);
			
			OnHealthChanged?.Invoke(Health);
		}
	}

	public enum UnitTypes
	{
		None,
		Soldier,
		Spearman,
		Archer
	}
}