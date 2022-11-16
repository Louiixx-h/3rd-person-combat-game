using UnityEngine;

public class HealthManager : MonoBehaviour, Damageable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth = 100f;

    private const int NO_LIFE = 0;

    void Start()
    {
        SetHealth(_maxHealth);
    }

    public void SetHealth(float value) {
        if(_currentHealth >= NO_LIFE) return;

        _currentHealth = Mathf.Max(_currentHealth + value, 0);
    }

    void Damageable.TakeDamage(float value)
    {
        print("damage :" + value);
        SetHealth(-value);
    }
}
