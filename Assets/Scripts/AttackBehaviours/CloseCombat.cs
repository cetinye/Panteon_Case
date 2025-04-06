using StrategyGameDemo.Interfaces;
using UnityEngine;

namespace StrategyGameDemo.AttackMonoBehaviours
{
	public class CloseCombat : MonoBehaviour, IAttackBehaviour
	{
		/// <summary>
		/// Deals damage to the attacked object.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="receiver"></param>
		/// <param name="range"></param>
		public void Attack(IDamageable attacker, IDamageable receiver, float range)
		{
			float distance = receiver.GetClosestDistance(transform.position);
			
			if (distance < range)
			{
				receiver.TakeDamage(attacker.GetDamage());
				Debug.LogWarning("Attack from: " + attacker + " to " + receiver + " damage: " + receiver.GetDamage() + " distance: " + distance);
			}
			else
				Debug.LogWarning($"Out Of Range! Difference: {(range - distance)}");
		}
	}
}