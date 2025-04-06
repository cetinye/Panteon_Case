using StrategyGameDemo.Controllers;
using UnityEngine;
using StrategyGameDemo.Models;
using StrategyGameDemo.Data;

namespace StrategyGameDemo.Factory
{
    public static class ConcreteUnitFactory
    {
        public static GameObject CreateUnitInstance(UnitTypes UnitType, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (!UnitFactory.GetUnitData(UnitType, out UnitSO data))
            {
                Debug.LogError($"ConcreteUnitFactory Error: Could not find UnitData for {UnitType}.");
                return null;
            }

            GameObject instance = GameObject.Instantiate(data.Prefab, new Vector3(position.x, position.y, 0), rotation, parent);
            instance.name = $"{data.UnitName}_{UnitType}";

            UnitController controller = instance.GetComponent<UnitController>();
            if (controller != null)
            {
                controller.Initialize();
            }

            return instance;
        }
    }
}