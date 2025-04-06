using StrategyGameDemo.Models;
using UnityEngine;

namespace StrategyGameDemo.Models
{
	[UnityEngine.Scripting.Preserve]
	public class BarrackModel : BuildingModel
	{
		public override BuildingTypes BuildingType => BuildingTypes.Barrack;
	}
}