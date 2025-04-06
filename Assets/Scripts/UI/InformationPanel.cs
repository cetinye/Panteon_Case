using StrategyGameDemo.Factory;
using StrategyGameDemo.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StrategyGameDemo.UI
{
	public class InformationPanel : MonoBehaviour
	{
		[Header("Building Information Components")] 
		[SerializeField] private GameObject buildingInformationPanel;
		[SerializeField] private TMP_Text buildingName;
		[SerializeField] private Image buildingImage;
		[SerializeField] private GameObject productionInformationPanel;
		[SerializeField] private UnitProductionInformation unitProductionInformation;
		private BuildingModel activeBuildingModel;
		
		[Header("Unit Information Components")]
		[SerializeField] private GameObject unitInformationPanel;
		[SerializeField] private TMP_Text unitName;
		[SerializeField] private Image unitImage;
		[SerializeField] private TMP_Text unitHP;
		[SerializeField] private TMP_Text unitAD;

		public void ShowBuildingInformation(BuildingModel buildingModel)
		{
			activeBuildingModel = buildingModel;
			ClearBuildingInformation();
			
			buildingName.text = buildingModel.BuildingName;
			buildingImage.sprite = buildingModel.BuildingSprite;

			if (buildingModel.ProducableUnits.Count > 0)
			{
				foreach (var producableUnit in buildingModel.ProducableUnits)
				{
					var spawnableUnit = Instantiate(unitProductionInformation, productionInformationPanel.transform);
					var spawnableModel = UnitFactory.GetUnit(producableUnit);
					spawnableUnit.Initialize(spawnableModel);
				}
			}
			
			SetUnitInformationState(false);
			SetBuildingInformationState(true);
		}

		public void ClearBuildingInformation()
		{
			for (int i = 0; i < productionInformationPanel.transform.childCount; i++)
			{
				Destroy(productionInformationPanel.transform.GetChild(i).gameObject);
			}
		}
		
		public void ClearInformationPanel()
		{
			for (int i = 0; i < productionInformationPanel.transform.childCount; i++)
			{
				Destroy(productionInformationPanel.transform.GetChild(i).gameObject);
			}
			
			SetBuildingInformationState(false);
		}

		private void SetBuildingInformationState(bool state)
		{
			buildingInformationPanel.SetActive(state);
		}
		
		public void ShowUnitInformation(UnitModel unitModel)
		{
			unitName.text = unitModel.UnitName;
			unitImage.sprite = unitModel.UnitSprite;
			unitHP.text = $"HP: {unitModel.Health:F0}";
			unitAD.text = $"AD: {unitModel.AttackDamage:F0}";
			
			SetBuildingInformationState(false);
			SetUnitInformationState(true);
		}
		
		private void SetUnitInformationState(bool state)
		{
			unitInformationPanel.SetActive(state);
		}
		
		public BuildingModel GetActiveBuildingModel()
		{
			return activeBuildingModel;
		}
	}
}