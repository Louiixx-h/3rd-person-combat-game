using Scripts.UI;
using UnityEngine;

namespace Scripts.Game.StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public InputReader InputReader { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public Targeter Targeter { get; private set; }
        [field: SerializeField] public Attack[] Attacks { get; private set; }
        [field: SerializeField] public EquippedWeapon EquippedWeapon { get; private set; }
        [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }
        [field: SerializeField] public float RunningSpeed { get; private set; }
        [field: SerializeField] public float FastRunningSpeed { get; private set; }
        [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float MaxLife { get; private set; } = 100;
        [field: SerializeField] public float CurrentLife { get; private set; }

        [HideInInspector] public Transform MainCamera;
        [HideInInspector] public bool IsTargeting;
        [HideInInspector] public int CurrentAttack;


        private void Start()
        {
            CurrentAttack = 0;
            MainCamera = Camera.main.transform;

            EquippedWeapon.SaveWeapon();

            UIManager.Instance.uiGameplay
                .SetMaxLife(MaxLife)
                .HealthManager(MaxLife);

            SwitchState(new PlayerGroundedState(this));
        }
    }
}