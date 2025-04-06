using System;
using StrategyGameDemo.Factory;
using StrategyGameDemo.Interfaces;
using StrategyGameDemo.Managers;
using StrategyGameDemo.Models;
using StrategyGameDemo.Views;
using UnityEngine;

namespace StrategyGameDemo
{
	public class UnitController : MonoBehaviour, ISelectBehaviour
	{
		[SerializeField] private UnitTypes unitType;
		
		private UnitModel model;
		private UnitView view;

		private PathFollow pathFollow;

		private IAttackBehaviour attackBehaviour;

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
			view.UpdateHealthText(model.Health);
			
			attackBehaviour = GetComponent<IAttackBehaviour>();
			
			pathFollow = GetComponent<PathFollow>();
			pathFollow.SetValues(model.MovementSpeed, model.RotationSpeed);
			
			model.OnHealthChanged += UpdateHealth;
		}
		
		public void LeftClick()
		{
			Select();
			UnitView.OnUnitSelect?.Invoke(model);
		}

		public void RightClick(Vector3 position)
		{
			if (model.IsSelected)
				MoveTo(position);
		}

		private void Select()
		{
			model.IsSelected = !model.IsSelected;
			view.SetSpriteColor(model.IsSelected ? Color.red : Color.white);

			if (model.IsSelected)
			{
				InputManager.OnRightClick += OnRightClicked;
			}
			else
			{
				InputManager.OnRightClick -= OnRightClicked;
			}
		}

		public void TakeDamage(float damage)
		{
			model.TakeDamage(damage);
		}

		public void Attack(UnitController unitToAttack)
		{
			attackBehaviour?.Attack(this, unitToAttack);
		}

		private void OnRightClicked(Vector3 position)
		{
			MoveTo(position);
		}

		public void MoveTo(Vector3 position)
		{
			pathFollow.SetDestination(position);
		}

		private void UpdateHealth(float health)
		{
			view.UpdateHealthText(health);
		}
	}
}