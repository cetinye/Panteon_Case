using StrategyGameDemo.Models;
using UnityEngine;

namespace StrategyGameDemo.Models
{
	[UnityEngine.Scripting.Preserve]
	public class PowerPlantModel : BuildingModel
	{
		public override BuildingTypes BuildingType => BuildingTypes.PowerPlant;
	}
}