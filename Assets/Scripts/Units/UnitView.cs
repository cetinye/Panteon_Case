using System;
using StrategyGameDemo.Managers;
using StrategyGameDemo.Models;
using TMPro;
using UnityEngine;

namespace StrategyGameDemo.Views
{
	public class UnitView : MonoBehaviour
	{
		[Header("Unit Display")]
		[SerializeField] private SpriteRenderer spriteRenderer;
		
		private void Awake()
		{
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		}

		public void SetUnitSprite(Sprite sprite)
		{
			spriteRenderer.sprite = sprite;
		}

		public void SetSpriteColor(Color color)
		{
			spriteRenderer.color = color;
		}
		
		public void ShowUnitInfo(UnitModel model)
		{
			UIManager.OnShowUnitInfo?.Invoke(model);
		}
	}
}