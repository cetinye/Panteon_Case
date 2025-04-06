namespace StrategyGameDemo.Interfaces
{
	public interface IDamageable
	{
		void TakeDamage(float damage);
		float GetDamage();
		float GetClosestDistance(UnityEngine.Vector3 position);
	}
}