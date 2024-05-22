using AlienWaves.CoreDI;
using CombatGame.Commons;
using CombatGame.Commons.GamePhysics;
using CombatGame.Commons.Health;
using CombatGame.Commons.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace CombatGame.Enemy
{
    public class NormalEnemyController : BaseEnemy, IDamageable
    {
        [SerializeField] 
        private float maxLife;

        [SerializeField] 
        private Image healthSprite;

        [SerializeField] 
        private NavMeshAgent agent;
        public NavMeshAgent Agent => agent;

        [SerializeField] 
        private CharacterController characterController;
        public CharacterController CharacterController => characterController;

        [SerializeField] 
        private ForceReceiver forceReceiver;
        public ForceReceiver ForceReceiver => forceReceiver;

        [SerializeField] private float movementSpeed;
        public float MovementSpeed => movementSpeed;

        public BasePlayer Player { get; set; }

        private IHealth _health;
        private bool hasPlayerAttached = false;
        private bool hasAgentAttached = false;

        private void Awake()
        {
            _health = new Health();
        }

        private void OnEnable()
        {
            _health.OnCurrentLifeChangeInPercentage += UpdateHealthBar;
            _health.OnActionDeath += OnDie;
        }

        private void OnDisable()
        {
            _health.OnCurrentLifeChangeInPercentage -= UpdateHealthBar;
            _health.OnActionDeath -= OnDie;
        }

        private void Start()
        {
            ServiceLocator.ForSceneOf(this).TryGet(out BasePlayer player);
            Player = player;
            hasAgentAttached = Agent != null;
            hasPlayerAttached = Player != null;
            _health.SetMaxLife(maxLife);
        }

        private void Update()
        {
            if (hasAgentAttached && Agent.isOnNavMesh && hasPlayerAttached)
            {
                Agent.destination = Player.transform.position;

                var direction = Agent.desiredVelocity.normalized * MovementSpeed;
                Move(direction, Time.deltaTime);
            }
            else
            {
                Move(Vector3.zero, Time.deltaTime);
            }
        }

        public void ApplyDamage(float amount)
        {
            _health.ApplyDamage(amount);
        }

        private void Move(Vector3 motion, float deltaTime)
        {
            Vector3 gravityApplied = ForceReceiver.Movement;
            CharacterController.Move((motion + gravityApplied) * deltaTime);
        }

        private void UpdateHealthBar(float amount)
        {
            healthSprite.fillAmount = Mathf.Clamp01(amount);
        }

        private void OnDie()
        {
            Destroy(gameObject);
        }
    }
}