using StrategyGameDemo.Controllers;
using UnityEngine;
using StrategyGameDemo.Models;
using StrategyGameDemo.Data;

namespace StrategyGameDemo.Factory
{
    public static class ConcreteBuildingFactory
    {
        public static GameObject CreateBuildingInstance(BuildingTypes buildingType, Vector3 position, Quaternion rotation, Transform parent = null, Node placedNode = null, bool isPreview = false)
        {
            if (!BuildingFactory.GetBuildingData(buildingType, out BuildingSO data))
            {
                Debug.LogError($"ConcreteBuildingFactory Error: Could not find BuildingData for {buildingType}.");
                return null;
            }

            GameObject instance = GameObject.Instantiate(data.Prefab, new Vector3(position.x, position.y, 0), rotation, parent);
            instance.name = $"{data.BuildingName}_{buildingType}";

            BuildingController controller = instance.GetComponent<BuildingController>();
            if (controller != null)
            {
                controller.Initialize(placedNode, isPreview);
            }

            return instance;
        }
    }
}