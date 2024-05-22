using UnityEngine;

namespace CombatGame.Player
{
    public class PlayerTargetingState : PlayerBaseState
    {
        private Vector3 _targetDirection;

        public PlayerTargetingState(PlayerController playerStateMachine) : base(playerStateMachine) { }

        public override void Enter()
        {
            if (!context.IsTargeting)
            {
                context.IsTargeting = true;
                context.Animator.CrossFadeInFixedTime("Unarmed Weapon", 0.1f);
            }
            else
            {
                context.Animator.CrossFadeInFixedTime("TargetingBlendTree", 0.1f);
            }
            context.InputReader.TargetEvent += HandleCancelTargeting;
            context.InputReader.RollEvent += Roll;
        }

        public override void Exit()
        {
            context.InputReader.TargetEvent -= HandleCancelTargeting;
            context.Animator.SetLayerWeight(1, 0);
            context.EquippedWeapon.SaveWeapon();
            context.InputReader.RollEvent -= Roll;
        }

        public override void Tick(float deltaTime)
        {
            if (context.InputReader.IsAttacking)
            {
                context.SwitchState(new PlayerAttackState(context, context.CurrentAttack));
                return;
            }

            if (context.Targeter.CurrentTarget == null)
            {
                context.SwitchState(new PlayerGroundedState(context));
                return;
            }

            _targetDirection = CalculateMovement();
            Move(_targetDirection * context.TargetingMovementSpeed, deltaTime);
            HandleAnimator(deltaTime);
            FaceTarget();
        }

        void HandleCancelTargeting()
        {
            context.IsTargeting = false;
            context.SwitchState(new PlayerGroundedState(context));
        }

        void HandleAnimator(float deltaTime)
        {
            if (context.InputReader.MovementValue.y == 0)
            {
                context.Animator.SetFloat("TargetingForward", 0, 0.1f, deltaTime);
            }
            else
            {
                float value = context.InputReader.MovementValue.y > 0 ? 1f : -1f;
                context.Animator.SetFloat("TargetingForward", value, 0.1f, deltaTime);
            }

            if (context.InputReader.MovementValue.x == 0)
            {
                context.Animator.SetFloat("TargetingRight", 0, 0.1f, deltaTime);
            }
            else
            {
                float value = context.InputReader.MovementValue.x > 0 ? 1f : -1f;
                context.Animator.SetFloat("TargetingRight", value, 0.1f, deltaTime);
            }
        }

        Vector3 CalculateMovement()
        {
            Vector3 movement = new Vector3();

            movement += context.transform.right * context.InputReader.MovementValue.x;
            movement += context.transform.forward * context.InputReader.MovementValue.y;
            movement.y = 0;

            return movement;
        }

        private void Roll()
        {
            context.SwitchState(new PlayerRollState(context, _targetDirection));
        }
    }
}