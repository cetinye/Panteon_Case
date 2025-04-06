namespace StrategyGameDemo.Interfaces
{
	public interface IAttackBehaviour
	{
		void Attack(IDamageable attacker, IDamageable receiver, float range);
	}
}