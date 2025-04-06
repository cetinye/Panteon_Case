using System;
using StrategyGameDemo.Managers;
using StrategyGameDemo.Models;
using UnityEngine;
using UnityEngine.UI;

namespace StrategyGameDemo.UI
{
	public class ProductionMenuElement : MonoBehaviour
	{
		[SerializeField] private BuildingTypes buildingType;
		private Button button;
		
		private void Start()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(() =>
			{
				UIManager.Instance.ClearBuildingInformation();
				PlacementManager.Instance.PreviewBuilding(buildingType);
			});
		}
	}
}