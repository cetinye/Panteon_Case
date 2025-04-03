using TMPro;
using UnityEngine;

namespace StrategyGameDemo.Views
{
	public class BuildingView : MonoBehaviour
	{
		[Header("Building Display")]
		[SerializeField] private SpriteRenderer spriteRenderer;
		
		[Header("Information Panel")]
		[SerializeField] private TMP_Text healthText;

		private void Awake()
		{
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		}

		public void SetBuildingSprite(Sprite sprite)
		{
			spriteRenderer.sprite = sprite;
		}

		public void SetBuildingSpriteSize(Vector2 size)
		{
			spriteRenderer.transform.localScale = new Vector3(size.x - 1, size.y - 1, 1f);
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