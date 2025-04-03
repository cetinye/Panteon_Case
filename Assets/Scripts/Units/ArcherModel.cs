using StrategyGameDemo.Interfaces;
using UnityEngine;

namespace StrategyGameDemo.Models
{
	public class ArcherModel : UnitModel
	{
		public override UnitTypes UnitType => UnitTypes.Archer;
		
		public ArcherModel()
		{
			Health = 10;
			AttackDamage = 5;
		}
	}
}