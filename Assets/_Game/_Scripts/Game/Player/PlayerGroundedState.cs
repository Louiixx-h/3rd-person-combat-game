using UnityEngine;

namespace CombatGame.Player
{
    public class PlayerGroundedState : PlayerBaseState
    {
        private bool _isGroundState = false;
        private Vector3 _targetDirection;

        public PlayerGroundedState(PlayerController stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            context.EquippedWeapon.SaveWeapon();
            context.InputReader.TargetEvent += HandleTarget;
            context.InputReader.JumpEvent += Jump;
            context.InputReader.RollEvent += Roll;
            context.Animator.CrossFadeInFixedTime("GroundedBlendTree", 0.1f);
        }

        public override void Exit()
        {
            context.Animator.SetFloat("MovementSpeed", 0);
            context.InputReader.TargetEvent -= HandleTarget;
            context.InputReader.JumpEvent -= Jump;
            context.InputReader.RollEvent -= Roll;
        }

        public override void Tick(float deltaTime)
        {
            if (context.InputReader.IsAttacking && context.Grounded)
            {
                context.SwitchState(new PlayerAttackState(context, context.CurrentAttack));
                return;
            }

            GroundedCheck();
            MovePlayer(deltaTime);

            if (context.Grounded)
            {
                if (!_isGroundState)
                {
                    _isGroundState = true;
                    context.Animator.CrossFadeInFixedTime("GroundedBlendTree", 0.1f);
                }
            }
            else 
            {
                if (_isGroundState)
                {
                    _isGroundState = false;
                    context.Animator.CrossFadeInFixedTime("Fall", 0.1f);
                }
            }
        }

        private void MovePlayer(float deltaTime)
        {
            var moveSpeed = context.MovementSpeed;
            if (context.InputReader.MovementValue == Vector2.zero)
            {
                context.Animator.SetFloat("MovementSpeed", 0, 0.1f, deltaTime);
                Move(deltaTime);
                return;
            } 
            else if (context.InputReader.MovementValue != Vector2.zero && context.InputReader.IsDodging)
            {
                moveSpeed *= 2;
                context.Animator.SetFloat("MovementSpeed", 1, 0.1f, deltaTime);
            }

            float horizontal = context.InputReader.MovementValue.x;
            float vertical = context.InputReader.MovementValue.y;
            float cameraAngle = context.MainCamera.transform.eulerAngles.y;

            var targetRotation = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + cameraAngle;
            float rotation = Mathf.SmoothDampAngle(
                context.transform.eulerAngles.y,
                targetRotation, 
                ref context.RotationSpeed,
                context.RotationSmoothTime
            );

            context.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            _targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
            Move(_targetDirection.normalized * moveSpeed, deltaTime);
            context.Animator.SetFloat("MovementSpeed", 0.5f, 0.1f, deltaTime);
        }

        private void HandleTarget()
        {
            if (!context.Targeter.SelectTarget()) return;
            context.SwitchState(new PlayerTargetingState(context));
        }

        private void Roll()
        {
            context.SwitchState(new PlayerRollState(context, _targetDirection));
        }
    }
}