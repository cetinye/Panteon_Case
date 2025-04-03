using StrategyGameDemo.Interfaces;
using UnityEngine;

namespace StrategyGameDemo.Models
{
	public class SoldierModel : UnitModel
	{
		public override UnitTypes UnitType => UnitTypes.Soldier;
		
		public SoldierModel()
		{
			Health = 10;
			AttackDamage = 10;
		}
	}
}