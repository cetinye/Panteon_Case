using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using StrategyGameDemo.Models;
using StrategyGameDemo.Data;

namespace StrategyGameDemo.Factory
{
    public static class BuildingFactory
    {
        private static Dictionary<BuildingTypes, Type> buildingModelTypes = new Dictionary<BuildingTypes, Type>();
        private static Dictionary<BuildingTypes, BuildingSO> buildingDataAssets = new Dictionary<BuildingTypes, BuildingSO>();

        private static bool IsInitialized = false;

        public static void Initialize()
        {
            if (IsInitialized) return;

            var loadedData = GameManager.Instance.GetBuildingsData();
            buildingDataAssets.Clear();
            foreach (var data in loadedData)
            {
                if (data != null && data.BuildingType != BuildingTypes.None && !buildingDataAssets.ContainsKey(data.BuildingType))
                {
                    buildingDataAssets.Add(data.BuildingType, data);
                    Debug.Log($"Loaded BuildingData for {data.BuildingType}");
                }
                else
                {
                     Debug.LogWarning($"Duplicate or invalid BuildingType found in BuildingData: {data?.name}");
                }
            }

            buildingModelTypes.Clear();
            var assembly = Assembly.GetAssembly(typeof(BuildingModel));
            var modelSubtypes = assembly.GetTypes()
                                     .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BuildingModel)));

            foreach (var type in modelSubtypes)
            {
                BuildingModel tempInstance = Activator.CreateInstance(type) as BuildingModel;
                if (tempInstance != null)
                {
                    BuildingTypes modelEnum = tempInstance.BuildingType;
                    if (modelEnum != BuildingTypes.None && !buildingModelTypes.ContainsKey(modelEnum))
                    {
                        buildingModelTypes.Add(modelEnum, type);
                        Debug.Log($"Discovered Model Type {type.Name} for {modelEnum}");

                        if (!buildingDataAssets.ContainsKey(modelEnum))
                        {
                            Debug.LogWarning($"No BuildingData asset found for discovered model type: {modelEnum} ({type.Name})");
                        }
                    }
                    else
                    {
                         Debug.LogWarning($"Duplicate or invalid BuildingType ({modelEnum}) defined in Model: {type.Name}");
                    }
                }
            }

            IsInitialized = true;
        }

        public static BuildingModel GetBuilding(BuildingTypes buildingType)
        {
            Initialize();

            if (buildingModelTypes.TryGetValue(buildingType, out Type type) &&
                buildingDataAssets.TryGetValue(buildingType, out BuildingSO data))
            {
                var buildingModel = Activator.CreateInstance(type) as BuildingModel;

                if (buildingModel != null)
                {
                    buildingModel.InitializeFromData(data);
                    return buildingModel;
                }
                else
                {
                     Debug.LogError($"Failed to cast instance of {type.Name} to BuildingModel.");
                }
            }
            else
            {
                 if (!buildingModelTypes.ContainsKey(buildingType))
                     Debug.LogError($"BuildingFactory Error: No model type found for BuildingType: {buildingType}");
                 if (!buildingDataAssets.ContainsKey(buildingType))
                     Debug.LogError($"BuildingFactory Error: No BuildingData asset found for BuildingType: {buildingType}. Make sure a BuildingData asset exists in Data folder and GameManager.");
            }

            return null;
        }
    }
}