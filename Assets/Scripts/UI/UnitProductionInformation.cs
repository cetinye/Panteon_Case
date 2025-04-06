using System;
using StrategyGameDemo.Factory;
using StrategyGameDemo.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StrategyGameDemo.UI
{
	public class UnitProductionInformation : MonoBehaviour
	{
		[Header("Unit Production Information")]
		[SerializeField] private TMP_Text unitName;
		[SerializeField] private Image unitImage;
		private UnitModel unitModel;

		[Header("Unit Production Button")]
		[SerializeField] private Button button;

		public static Action<UnitModel> OnUnitSpawnClick;

		public void Initialize(UnitModel model)
		{
			unitName.text = model.UnitName;
			unitImage.sprite = model.UnitSprite;
			unitModel = model;
			
			button.onClick.AddListener(() => OnUnitSpawnClick?.Invoke(unitModel));
		}
	}
}