using System;
using TMPro;
using UnityEngine;

namespace StrategyGameDemo.Views
{
	public class UnitView : MonoBehaviour
	{
		[Header("Unit Display")]
		[SerializeField] private SpriteRenderer spriteRenderer;
		
		[Header("Information Panel")]
		[SerializeField] private TMP_Text healthText;

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

		public void UpdateHealthText(float health)
		{
			if (healthText != null)
				healthText.text = $"HP: {health.ToString("F0")}";
		}
	}
}