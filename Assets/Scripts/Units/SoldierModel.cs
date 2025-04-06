using StrategyGameDemo.Interfaces;
using UnityEngine;

namespace StrategyGameDemo.Models
{
	[UnityEngine.Scripting.Preserve]
	public class SoldierModel : UnitModel
	{
		public override UnitTypes UnitType => UnitTypes.Soldier;
	}
}