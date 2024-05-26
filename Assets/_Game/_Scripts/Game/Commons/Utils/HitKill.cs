using CombatGame.Commons.Interfaces;
using UnityEngine;

namespace CombatGame.Commons.Utils
{
    class HitKill : MonoBehaviour
    {
        [SerializeField] private float damageAmount = 10000000;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(damageAmount);
            }
        }
    }
}
