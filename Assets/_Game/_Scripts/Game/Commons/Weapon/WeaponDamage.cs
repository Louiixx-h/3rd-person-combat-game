using CombatGame.Commons.GamePhysics;
using CombatGame.Commons.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace CombatGame.Commons.Weapons
{
    public class WeaponDamage : MonoBehaviour
    {
        [SerializeField] private Collider _myCollider;
        private readonly List<Collider> _colliders = new();
        private float _damage;
        private float _knockback;

        private void OnEnable()
        {
            _colliders.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == _myCollider) return;
            if (_colliders.Contains(other)) return;

            _colliders.Add(other);

            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(_damage);
            }

            if (other.gameObject.TryGetComponent(out ForceReceiver forceReceiver))
            {
                var direction = other.gameObject.transform.position - _myCollider.transform.position;
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



