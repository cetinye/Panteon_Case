using StrategyGameDemo.Interfaces;
using UnityEngine;

namespace StrategyGameDemo
{
    public class RangedAttack : MonoBehaviour, IAttackBehaviour
    {
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