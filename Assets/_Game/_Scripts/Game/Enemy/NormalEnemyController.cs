using CombatGame.Commons;
using CombatGame.Commons.GamePhysics;
using CombatGame.Commons.Health;
using CombatGame.Commons.Interfaces;
using LuisLabs.CoreDI;
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

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float distanceAttack;

        [SerializeField]
        private float distanceVision;

        [SerializeField] 
        private CharacterController characterController;

        [SerializeField] 
        private ForceReceiver forceReceiver;

        [SerializeField] 
        private float movementSpeed;

        public BasePlayer Player { get; set; }

        private IHealth _health;
        private bool hasPlayerAttached = false;
        private bool hasAnimatorAttached = false;
        private bool hasAgentAttached = false;
        private bool isIdle = false;
        private bool isRunning = false;
        private bool isTakingHit = false;
        private bool isDead = false;

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
            hasAgentAttached = agent != null;
            hasPlayerAttached = Player != null;
            hasAnimatorAttached = animator != null;
            _health.SetMaxLife(maxLife);
        }

        private void Update()
        {
            if (!hasAgentAttached || !hasAnimatorAttached || !hasPlayerAttached || isTakingHit || isDead)
            {
                Move(Vector3.zero, Time.deltaTime);
                return;
            }

            if (Vector3.Distance(Player.transform.position, transform.position) > distanceVision || Vector3.Distance(Player.transform.position, transform.position) < distanceAttack)
            {
                if (!isIdle)
                {
                    isIdle = true;
                    animator.CrossFadeInFixedTime("idle", 0.1f);
                }
                isRunning = false;
                Move(Vector3.zero, Time.deltaTime);
            } 
            else
            {
                if (agent.isOnNavMesh)
                {
                    agent.destination = Player.transform.position;

                    var direction = agent.desiredVelocity.normalized * movementSpeed;
                    Move(direction, Time.deltaTime);

                    if (!isRunning)
                    {
                        isRunning = true;
                        animator.CrossFadeInFixedTime("run", 0.1f);
                    }
                    isIdle = false;
                }
            }
        }

        public void ApplyDamage(float amount)
        {
            isTakingHit = true;
            _health.ApplyDamage(amount);

            if (hasAnimatorAttached && !isDead)
            {
                animator.CrossFadeInFixedTime("take_hit", 0.1f);
            }
        }

        private void Move(Vector3 motion, float deltaTime)
        {
            Vector3 gravityApplied = forceReceiver.Movement;
            characterController.Move((motion + gravityApplied) * deltaTime);
        }

        private void UpdateHealthBar(float amount)
        {
            healthSprite.fillAmount = Mathf.Clamp01(amount);
        }

        private void OnDie()
        {
            isDead = true;

            if (hasAnimatorAttached)
            {
                animator.CrossFadeInFixedTime("die", 0.1f);
            }
        }

        public void EndTakeHitAnimation()
        {
            isTakingHit = false;
        }

        public void EndDieAnimation()
        {
            Destroy(gameObject);
        }
    }
}