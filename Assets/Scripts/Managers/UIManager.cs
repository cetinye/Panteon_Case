using System;
using StrategyGameDemo.Models;
using StrategyGameDemo.UI;
using StrategyGameDemo.Views;
using UnityEngine;

namespace StrategyGameDemo.Managers
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private InformationPanel informationPanel;

		private void OnEnable()
		{
			BuildingView.OnBuildingSelect += ShowBuildingInformation;
			UnitView.OnUnitSelect += ShowUnitInformation;
		}

		private void OnDisable()
		{
			BuildingView.OnBuildingSelect -= ShowBuildingInformation;
			UnitView.OnUnitSelect -= ShowUnitInformation;
		}

		private void ShowBuildingInformation(BuildingModel buildingModel)
		{
			informationPanel.ShowBuildingInformation(buildingModel);
		}
		
		private void ShowUnitInformation(UnitModel unitModel)
		{
			informationPanel.ShowUnitInformation(unitModel);
		}
	}
}