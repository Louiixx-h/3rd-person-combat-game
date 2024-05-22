using CombatGame.Commons.Interfaces;
using System;

namespace CombatGame.Commons.Health
{
    public interface IHealth : IDamageable, ICurable
    {
        const float NoLife = 0;

        Action OnActionDeath { get; set; }
        Action<float> OnCurrentLifeChange { get; set; }
        public Action<float> OnCurrentLifeChangeInPercentage { get; set; }
        float CurrentLife { get; set; }
        float MaxLife { get; set; }

        void HandleHealth(float value);
        void SetMaxLife(float life);
    }
}
