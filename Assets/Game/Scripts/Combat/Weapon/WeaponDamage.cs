using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider _myCollider;
    private List<Collider> _colliders = new List<Collider>();
    private float _damage;

    private void OnEnable() {
        _colliders.Clear();
    }

    private void OnTriggerEnter(Collider other) {
        if(other == _myCollider) return;
        if(_colliders.Contains(other)) return;

        _colliders.Add(other);

        if(other.gameObject.TryGetComponent<Damageable>(out Damageable damageable)) {
            damageable.TakeDamage(_damage);
        }
    }

    public void SetDamage(float value) 
    {
        _damage = value;
    }
}




