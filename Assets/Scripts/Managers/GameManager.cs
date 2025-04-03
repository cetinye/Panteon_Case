using System;
using System.Collections.Generic;
using StrategyGameDemo.Data;
using StrategyGameDemo.Models;
using UnityEngine;

namespace StrategyGameDemo
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance;
		
		[SerializeField] private GridController gridController;
		
		[SerializeField] private Camera mainCamera;
		
		[Header("Unit SO")]
		[SerializeField] private List<UnitSO> unitSOList = new List<UnitSO>();

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
			// UnitFactory.Initialize();
			// gridController.InitializeGrid();
		}

		[ContextMenu("Start Game")]
		public void Initialize()
		{
			UnitFactory.Initialize();
			gridController.InitializeGrid();
		}

		public void SpawnSoldier()
		{
			UnitFactory.GetUnit(UnitTypes.Soldier);
		}
		
		public void SpawnArcher()
		{
			UnitFactory.GetUnit(UnitTypes.Archer);
		}

		public Camera GetMainCamera()
		{
			return mainCamera;
		}

		public List<UnitSO> GetUnitsData()
		{
			return unitSOList;
		}
	}
}