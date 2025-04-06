using System;
using System.Collections;
using System.Collections.Generic;
using StrategyGameDemo.Controllers;
using StrategyGameDemo.Data;
using StrategyGameDemo.Factory;
using StrategyGameDemo.Models;
using UnityEngine;

namespace StrategyGameDemo.Managers
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance;
		
		[SerializeField] private GridController gridController;
		[SerializeField] private Camera mainCamera;
		
		[Header("Unit SO")]
		[SerializeField] private List<UnitSO> unitSOList = new List<UnitSO>();
		
		[Header("Building SO")]
		[SerializeField] private List<BuildingSO> buildingSOList = new List<BuildingSO>();

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

		private void Start()
		{
			Initialize();
		}

		private void Initialize()
		{
			BuildingFactory.Initialize();
			UnitFactory.Initialize();
			gridController.InitializeGrid();
		}

		public Camera GetMainCamera()
		{
			return mainCamera;
		}

		public List<UnitSO> GetUnitsData()
		{
			return unitSOList;
		}

		public List<BuildingSO> GetBuildingsData()
		{
			return buildingSOList;
		}
	}
}