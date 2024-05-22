using UnityEngine;
using CombatGame.Commons;
using CombatGame.Commons.GamePhysics;
using CombatGame.Commons.Weapons;
using CombatGame.Commons.StateMachine;
using CombatGame.Commons.Combat;
using AlienWaves.CoreDI;

namespace CombatGame.Player
{
    public class PlayerController : BasePlayer
    {
        [Header("Player Grounded")]
        [SerializeField]
        private bool grounded = true;
        public bool Grounded { get => grounded; set => grounded = value; }

        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        [SerializeField]
        private float groundedOffset = -0.14f;
        public float GroundedOffset => groundedOffset;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        [SerializeField]
        private float groundedRadius = 0.28f;
        public float GroundedRadius => groundedRadius;

        [Tooltip("What layers the character uses as ground")]
        [SerializeField]
        private LayerMask groundLayers;
        public LayerMask GroundLayers => groundLayers;

        [Tooltip("How fast the character turns to face movement direction")]
        [SerializeField]
        private float rotationSmoothTime = 0.12f;
        public float RotationSmoothTime => rotationSmoothTime;

        [Header("Weapon")]
        [SerializeField]
        private Attack[] attacks;
        public Attack[] Attacks => attacks;

        [SerializeField]
        private WeaponHandler weaponHandler;
        public WeaponHandler WeaponHandler => weaponHandler;

        [SerializeField]
        private WeaponSwitcher equippedWeapon;
        public WeaponSwitcher EquippedWeapon => equippedWeapon;

        [SerializeField]
        private WeaponDamage weaponDamage;
        public WeaponDamage WeaponDamage => weaponDamage;

        [Header("Movement")]
        [SerializeField]
        private CharacterController characterController;
        public CharacterController CharacterController => characterController;

        [SerializeField]
        private float movementSpeed;
        public float MovementSpeed => movementSpeed;

        [SerializeField]
        private float targetingMovementSpeed;
        public float TargetingMovementSpeed => targetingMovementSpeed;

        public float RotationSpeed;

        [SerializeField]
        private Animator animator;
        public Animator Animator => animator;

        [SerializeField]
        private InputReader inputReader;
        public InputReader InputReader => inputReader;

        [SerializeField]
        private ForceReceiver forceReceiver;
        public ForceReceiver ForceReceiver => forceReceiver;

        [SerializeField]
        private Targeter targeter;
        public Targeter Targeter => targeter;

        public bool IsTargeting { get; set; }
        [HideInInspector] public Transform MainCamera;
        [HideInInspector] public int CurrentAttack;

        private BaseState _currentState;

        private void Awake()
        {
            ServiceLocator.ForSceneOf(this).Register<BasePlayer>(this);
        }

        private void Start()
        {
            CurrentAttack = 0;
            MainCamera = Camera.main.transform;
            EquippedWeapon.SaveWeapon();
            SwitchState(new PlayerGroundedState(this));
        }

        public void SwitchState(BaseState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        private void Update()
        {
            _currentState?.Tick(Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }
    }
}