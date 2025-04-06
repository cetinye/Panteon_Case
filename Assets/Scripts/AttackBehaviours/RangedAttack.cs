using StrategyGameDemo.Interfaces;
using UnityEngine;

namespace StrategyGameDemo.AttackMonoBehaviours
{
    public class RangedAttack : MonoBehaviour, IAttackBehaviour
    {
        /// <summary>
        /// Deals damage to the attacked object from a distance. 
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="receiver"></param>
        /// <param name="range"></param>
        // For now, it's the same as CloseCombat but in future we can spawn bullets etc. to differ from CloseCombat.
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