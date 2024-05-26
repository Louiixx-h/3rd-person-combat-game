using UnityEngine;
using CombatGame.Commons;
using CombatGame.Commons.GamePhysics;
using CombatGame.Commons.Weapons;
using CombatGame.Commons.StateMachine;
using CombatGame.Commons.Combat;
using LuisLabs.CoreDI;
using CombatGame.Commons.Health;
using UnityEngine.UI;
using LuisLabs.CoreLevel;
using CombatGame.Commons.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CombatGame.Player
{
    public class PlayerController : BasePlayer, IDamageable
    {
        [SerializeField]
        private float maxLife;

        [SerializeField]
        private Image healthSprite;

        [SerializeField]
        private InputReader inputReader;

        [SerializeField]
        private ForceReceiver forceReceiver;

        [SerializeField]
        private Targeter targeter;

        #region Animation
        
        [Header("Animation")]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private List<AnimationClip> animationClips = new()
        {
            new() {Key = AnimationName.Grounded, Value = "GroundedBlendTree"},
            new() {Key = AnimationName.Targeting, Value = "TargetingBlendTree"},
            new() {Key = AnimationName.Jump, Value = "Jump"},
            new() {Key = AnimationName.Fall, Value = "Fall"},
            new() {Key = AnimationName.Roll, Value = "Roll"},
            new() {Key = AnimationName.Death, Value = "Death"},
        };

        #endregion

        #region Player Grounded
        [Header("Player Grounded")]
        [SerializeField]
        private bool grounded = true;

        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        [SerializeField]
        private float groundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        [SerializeField]
        private float groundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        [SerializeField]
        private LayerMask groundLayers;

        [Tooltip("How fast the character turns to face movement direction")]
        [SerializeField]
        private float rotationSmoothTime = 0.12f;
        #endregion

        #region Weapon
        [Header("Weapon")]
        [SerializeField]
        private Attack[] attacks;

        [SerializeField]
        private WeaponHandler weaponHandler;

        [SerializeField]
        private WeaponSwitcher weaponSwitcher;

        [SerializeField]
        private WeaponSwitcher shieldSwitcher;

        [SerializeField]
        private WeaponDamage weaponDamage;
        #endregion

        #region Movement
        [Header("Movement")]
        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private float movementSpeed;

        [SerializeField]
        private float runSpeed = 1.5f;

        [SerializeField]
        private float jumpForce;

        [SerializeField]
        private float targetingMovementSpeed;

        public float RotationSpeed;
        #endregion

        #region Getters and Setters
        public Dictionary<AnimationName, string> AnimationClips { get; set; }
        public int CurrentAttack { get; set; }
        public Animator Animator => animator;
        public InputReader InputReader => inputReader;
        public ForceReceiver ForceReceiver => forceReceiver;
        public Targeter Targeter => targeter;
        public bool Grounded { get => grounded; set => grounded = value; }
        public float GroundedOffset => groundedOffset;
        public float GroundedRadius => groundedRadius;
        public LayerMask GroundLayers => groundLayers;
        public float RotationSmoothTime => rotationSmoothTime;
        public Attack[] Attacks => attacks;
        public WeaponHandler WeaponHandler => weaponHandler;
        public WeaponSwitcher WeaponSwitcher => weaponSwitcher;
        public WeaponSwitcher ShieldSwitcher => shieldSwitcher;
        public WeaponDamage WeaponDamage => weaponDamage;
        public CharacterController CharacterController => characterController;
        public float MovementSpeed => movementSpeed;
        public float RunSpeed => runSpeed;
        public float JumpForce => jumpForce;
        public float TargetingMovementSpeed => targetingMovementSpeed;
        public Camera MainCamera { get; set; }
        
        #endregion

        private BaseState _currentState;
        private IHealth _health;
        private ISceneController _sceneController;

        private void Awake()
        {
            AnimationClips = animationClips.ToDictionary(clip => clip.Key, clip => clip.Value);
            _health = new Health();
            _health.SetMaxLife(maxLife);
            ServiceLocator.ForSceneOf(this).Register<BasePlayer>(this);
        }

        private void Start()
        {
            _sceneController = FindFirstObjectByType<SceneController>();
            _health.OnCurrentLifeChangeInPercentage += UpdateHealthBar;
            _health.OnActionDeath += OnDie;
            CurrentAttack = 0;
            MainCamera = Camera.main;
            ShieldSwitcher.Save();
            SwitchState(new PlayerGroundedState(this));
            
        }

        private void OnDestroy()
        {
            _health.OnCurrentLifeChangeInPercentage -= UpdateHealthBar;
            _health.OnActionDeath -= OnDie;
        }

        private void Update()
        {
            _currentState?.Tick(Time.deltaTime);
        }

        public void SwitchState(BaseState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public void ApplyDamage(float amount)
        {
            _health.ApplyDamage(amount);
        }

        public void SwitchWeapons()
        {
            WeaponSwitcher.Use();
        }

        private void UpdateHealthBar(float amount)
        {
            healthSprite.fillAmount = Mathf.Clamp01(amount);
        }

        private void OnDie()
        {
            _sceneController.SwitchScene("PrototypeMechanicsScene");
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