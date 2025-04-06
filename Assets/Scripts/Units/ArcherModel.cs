using StrategyGameDemo.Interfaces;
using UnityEngine;

namespace StrategyGameDemo.Models
{
	[UnityEngine.Scripting.Preserve]
	public class ArcherModel : UnitModel
	{
		public override UnitTypes UnitType => UnitTypes.Archer;
		
	}
}