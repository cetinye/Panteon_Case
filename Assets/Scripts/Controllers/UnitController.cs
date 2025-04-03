using System;
using StrategyGameDemo.Interfaces;
using StrategyGameDemo.Models;
using StrategyGameDemo.Views;
using UnityEngine;

namespace StrategyGameDemo
{
	public class UnitController : MonoBehaviour, ISelectBehaviour
	{
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
			model = UnitFactory.GetUnit(UnitTypes.Soldier);

			view = GetComponent<UnitView>();
			view.SetUnitSprite(model.UnitSprite);
			view.UpdateHealthText(model.Health);
			
			attackBehaviour = GetComponent<IAttackBehaviour>();
			
			pathFollow = GetComponent<PathFollow>();
			
			model.OnHealthChanged += UpdateHealth;
		}
		
		public void LeftClick()
		{
			print("Left Clicked Unit");
			Select();
			print(model.IsSelected);
		}

		public void RightClick(Vector3 position)
		{
			if (model.IsSelected)
				pathFollow.SetDestination(position);
		}

		private void Select()
		{
			model.IsSelected = !model.IsSelected;
			view.SetSpriteColor(model.IsSelected ? Color.red : Color.white);
		}

		public void TakeDamage(float damage)
		{
			model.TakeDamage(damage);
		}

		public void Attack(UnitController unitToAttack)
		{
			attackBehaviour?.Attack(this, unitToAttack);
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