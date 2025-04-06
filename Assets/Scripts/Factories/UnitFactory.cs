using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using StrategyGameDemo.Models;
using StrategyGameDemo.Data;

namespace StrategyGameDemo.Factory
{
    public static class UnitFactory
    {
        private static Dictionary<UnitTypes, Type> unitModelTypes = new Dictionary<UnitTypes, Type>();
        private static Dictionary<UnitTypes, UnitSO> unitDataAssets = new Dictionary<UnitTypes, UnitSO>();

        private static bool IsInitialized = false;

        public static void Initialize()
        {
            if (IsInitialized) return;

            var loadedData = GameManager.Instance.GetUnitsData();
            unitDataAssets.Clear();
            foreach (var data in loadedData)
            {
                if (data != null && data.UnitType != UnitTypes.None && !unitDataAssets.ContainsKey(data.UnitType))
                {
                    unitDataAssets.Add(data.UnitType, data);
                    Debug.Log($"Loaded UnitData for {data.UnitType}");
                }
                else
                {
                     Debug.LogWarning($"Duplicate or invalid UnitType found in UnitData: {data?.name}");
                }
            }

            unitModelTypes.Clear();
            var assembly = Assembly.GetAssembly(typeof(UnitModel));
            var modelSubtypes = assembly.GetTypes()
                                     .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(UnitModel)));

            foreach (var type in modelSubtypes)
            {
                UnitModel tempInstance = Activator.CreateInstance(type) as UnitModel;
                if (tempInstance != null)
                {
                    UnitTypes modelEnum = tempInstance.UnitType;
                    if (modelEnum != UnitTypes.None && !unitModelTypes.ContainsKey(modelEnum))
                    {
                        unitModelTypes.Add(modelEnum, type);
                        Debug.Log($"Discovered Model Type {type.Name} for {modelEnum}");

                        if (!unitDataAssets.ContainsKey(modelEnum))
                        {
                            Debug.LogWarning($"No UnitData asset found for discovered model type: {modelEnum} ({type.Name})");
                        }
                    }
                    else
                    {
                         Debug.LogWarning($"Duplicate or invalid UnitType ({modelEnum}) defined in Model: {type.Name}");
                    }
                }
            }

            IsInitialized = true;
        }

        public static UnitModel GetUnit(UnitTypes unitType)
        {
            Initialize();

            if (unitModelTypes.TryGetValue(unitType, out Type type) &&
                unitDataAssets.TryGetValue(unitType, out UnitSO data))
            {
                var unitModel = Activator.CreateInstance(type) as UnitModel;

                if (unitModel != null)
                {
                    unitModel.InitializeFromData(data);
                    return unitModel;
                }
                else
                {
                     Debug.LogError($"Failed to cast instance of {type.Name} to UnitModel.");
                }
            }
            else
            {
                 if (!unitModelTypes.ContainsKey(unitType))
                     Debug.LogError($"UnitFactory Error: No model type found for UnitType: {unitType}");
                 if (!unitDataAssets.ContainsKey(unitType))
                     Debug.LogError($"UnitFactory Error: No UnitData asset found for UnitType: {unitType}. Make sure a UnitData asset exists in Data folder and GameManager.");
            }

            return null;
        }
        
        public static bool GetUnitData(UnitTypes unitType, out UnitSO data)
        {
            Initialize();
            if (unitDataAssets.TryGetValue(unitType, out data))
            {
                return data != null;
            }
            
            return false;
        }
    }
}