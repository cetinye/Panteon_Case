using System;
using StrategyGameDemo.Factory;
using StrategyGameDemo.Interfaces;
using StrategyGameDemo.Managers;
using StrategyGameDemo.Models;
using StrategyGameDemo.Views;
using UnityEngine;

namespace StrategyGameDemo.Controllers
{
	public class UnitController : MonoBehaviour, ISelectBehaviour, IDamageable
	{
		[SerializeField] private UnitTypes unitType;
		
		private UnitModel model;
		private UnitView view;

		private PathFollow pathFollow;

		private IAttackBehaviour attackBehaviour;
		private IDamageable damageable;

		private void Start()
		{
			Initialize();
		}

		private void OnDisable()
		{
			model.OnHealthChanged -= UpdateHealth;
		}

		public void Initialize()
		{
			model = UnitFactory.GetUnit(unitType);

			view = GetComponent<UnitView>();
			view.SetUnitSprite(model.UnitSprite);
			
			attackBehaviour = GetComponent<IAttackBehaviour>();
			damageable = GetComponent<IDamageable>();
			
			pathFollow = GetComponent<PathFollow>();
			pathFollow.SetValues(model.MovementSpeed, model.RotationSpeed);
			
			model.OnHealthChanged += UpdateHealth;
		}
		
		public void LeftClick()
		{
			Select();
			view.ShowUnitInfo(model);
		}

		public void RightClick()
		{
			
		}

		private void Select()
		{
			model.IsSelected = !model.IsSelected;
			view.SetSpriteColor(model.IsSelected ? Color.red : Color.white);

			if (model.IsSelected)
			{
				InputManager.OnRightClick += Move;
				InputManager.OnRightClickUnit += Combat;

			}
			else
			{
				InputManager.OnRightClick -= Move;
				InputManager.OnRightClickUnit -= Combat;
			}
		}

		public void TakeDamage(float damage)
		{
			model.TakeDamage(damage);
			
			if (model.Health <= 0)
			{
				gameObject.SetActive(false);
			}
		}

		public float GetDamage()
		{
			return model.AttackDamage;
		}

		public void Attack(IDamageable unitToAttack)
		{
			attackBehaviour?.Attack(damageable, unitToAttack, model.Range);
		}

		private void Move(Vector3 position)
		{
			MoveTo(position);
		}

		private void Combat(IDamageable unitToAttack)
		{
			Attack(unitToAttack);
		}

		private void MoveTo(Vector3 position)
		{
			pathFollow.SetDestination(position);
		}

		private void UpdateHealth(float health)
		{
			UIManager.Instance.UpdateUnitHealth(health, model);
		}

		public float GetClosestDistance(Vector3 position)
		{
			return Vector3.Distance(transform.position, position);
		}
	}
}