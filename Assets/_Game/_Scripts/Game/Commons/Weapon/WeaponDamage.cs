using CombatGame.Commons.GamePhysics;
using CombatGame.Commons.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatGame.Commons.Weapons
{
    public class WeaponDamage : MonoBehaviour
    {
        [SerializeField] private Collider myCollider;
        [SerializeField] private UnityEvent onDamageApplied;
        private readonly List<Collider> _colliders = new();
        private float _damage;
        private float _knockback;

        private void OnEnable()
        {
            _colliders.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == myCollider) return;
            if (_colliders.Contains(other)) return;

            _colliders.Add(other);

            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                onDamageApplied?.Invoke();
                damageable.ApplyDamage(_damage);
            }

            if (other.gameObject.TryGetComponent(out ForceReceiver forceReceiver))
            {
                var direction = other.gameObject.transform.position - myCollider.transform.position;
                forceReceiver.AddForce(direction.normalized * _knockback);
            }
        }

        public void SetDamage(float value, float knockback)
        {
            _damage = value;
            _knockback = knockback;
        }
    }
}



