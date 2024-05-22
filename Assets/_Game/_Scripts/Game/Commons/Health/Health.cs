using System;
using UnityEngine;

namespace CombatGame.Commons.Health
{
    public class Health : IHealth
    {
        public Action OnActionDeath { get; set; }
        public Action<float> OnCurrentLifeChange { get; set; }
        public Action<float> OnCurrentLifeChangeInPercentage { get; set; }
        public float CurrentLife { get; set; }
        public float MaxLife { get; set; }

        private bool _isDead;

        public void HandleHealth(float value)
        {
            CurrentLife = Mathf.Clamp(CurrentLife + value, IHealth.NoLife, MaxLife);
            OnCurrentLifeChange?.Invoke(CurrentLife);
            OnCurrentLifeChangeInPercentage?.Invoke(Mathf.Clamp01(CurrentLife / MaxLife));

            if (CurrentLife > IHealth.NoLife) return;
            
            _isDead = true;
            OnActionDeath?.Invoke();
        }

        public void SetMaxLife(float life)
        {
            if (_isDead) return;
            MaxLife = life;
            var result = Mathf.Clamp(MaxLife, IHealth.NoLife, MaxLife);
            HandleHealth(+result);
        }

        public void ApplyCure(float life)
        {
            if (_isDead) return;
            var result = Mathf.Clamp(life, IHealth.NoLife, MaxLife);
            HandleHealth(+result);
        }

        public void ApplyDamage(float damage)
        {
            if (_isDead) return;
            var result = Mathf.Clamp(damage, IHealth.NoLife, MaxLife);
            HandleHealth(-result);
        }
    }
}