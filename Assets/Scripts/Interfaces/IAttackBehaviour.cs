namespace StrategyGameDemo.Interfaces
{
	public interface IAttackBehaviour
	{
		void Attack(UnitController attacker, UnitController receiver);
	}
}