using System;
using StrategyGameDemo.Models;
using StrategyGameDemo.UI;
using StrategyGameDemo.Views;
using UnityEngine;

namespace StrategyGameDemo.Managers
{
	public class UIManager : MonoBehaviour
	{
		public static UIManager Instance;
		
		[SerializeField] private InformationPanel informationPanel;

		public static Action<BuildingModel> OnShowBuildingInfo;
		public static Action<UnitModel> OnShowUnitInfo;

		private void Awake()
		{
			if (Instance != null && Instance != this) 
			{ 
				Destroy(this); 
			} 
			else 
			{ 
				Instance = this; 
			}
		}

		private void OnEnable()
		{
			OnShowBuildingInfo += ShowBuildingInformation;
			OnShowUnitInfo += ShowUnitInformation;
		}

		private void OnDisable()
		{
			OnShowBuildingInfo -= ShowBuildingInformation;
			OnShowUnitInfo -= ShowUnitInformation;
		}

		private void ShowBuildingInformation(BuildingModel buildingModel)
		{
			informationPanel.ShowBuildingInformation(buildingModel);
		}
		
		private void ShowUnitInformation(UnitModel unitModel)
		{
			informationPanel.ShowUnitInformation(unitModel);
		}

		public void ClearBuildingInformation()
		{
			informationPanel.ClearInformationPanel();
		}

		public BuildingModel GetActiveBuildingModel()
		{
			return informationPanel.GetActiveBuildingModel();
		}
	}
}