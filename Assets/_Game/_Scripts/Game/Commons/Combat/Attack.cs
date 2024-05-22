using System;

namespace CombatGame.Commons.Combat
{
    [Serializable]
    public struct Attack
    {
        public string AttackName;
        public float TransitionDuration;
        public float ComboAttackTime;
        public float ForceTime;
        public float Force;
        public int ComboStateIndex;
        public int Damage;
        public int KnockBack;
    }
}