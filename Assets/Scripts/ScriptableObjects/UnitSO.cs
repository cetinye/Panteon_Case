using StrategyGameDemo.Models;
using UnityEngine;

namespace StrategyGameDemo.Data
{
    [CreateAssetMenu(fileName = "NewUnitData", menuName = "StrategyDemo/Unit Data")]
    public class UnitSO : ScriptableObject
    {
        public UnitTypes UnitType;

        [Header("Display")]
        public Sprite UnitSprite;

        [Header("Stats")]
        public float Health = 10f;
        public float AttackDamage = 5f;
        public float Range = 1f;
    }
}