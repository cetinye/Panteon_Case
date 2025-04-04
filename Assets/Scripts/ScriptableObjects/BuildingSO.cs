using System.Collections.Generic;
using StrategyGameDemo.Models;
using UnityEngine;

namespace StrategyGameDemo.Data
{
    [CreateAssetMenu(fileName = "NewBuildingData", menuName = "StrategyDemo/Building Data")]
    public class BuildingSO : ScriptableObject
    {
        public BuildingTypes BuildingType;

        [Header("Display")]
        public Sprite BuildingSprite;

        [Header("Stats")] 
        public string BuildingName;
        public Vector2 BuildingSize;
        public float Health = 100f;
        public List<UnitTypes> ProducableUnits;
    }
}