using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Game.StateMachine.Player
{
    public class PlayerGroundedState : PlayerBaseState
    {
        private bool _isDodgePressed = false;

        public PlayerGroundedState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            playerStateMachine.InputReader.TargetEvent += OnTarget;
            playerStateMachine.InputReader.DodgeEvent += OnDodge;
            playerStateMachine.InputReader.JumpEvent += OnJump;

            playerStateMachine.Animator.CrossFadeInFixedTime("GroundedBlendTree", 0.1f);
        }

        public override void Exit()
        {
            playerStateMachine.InputReader.TargetEvent -= OnTarget;
            playerStateMachine.InputReader.DodgeEvent -= OnDodge;
            playerStateMachine.InputReader.JumpEvent -= OnJump;

            playerStateMachine.Animator.SetFloat("MovementSpeed", 0);
        }

        public override void Tick(float deltaTime)
        {
            if (playerStateMachine.InputReader.IsAttacking)
            {
                playerStateMachine.SwitchState(new PlayerAttackState(playerStateMachine, playerStateMachine.CurrentAttack));
                return;
            }

            HandleMove(deltaTime);
        }

        private void HandleMove(float deltaTime)
        {
            var movement = CalculateMovement(deltaTime);
            
            if (playerStateMachine.InputReader.MovementValue == Vector2.zero)
            {
                HandleIdle(deltaTime);
                return;
            }

            if(_isDodgePressed)
            {
                HandleFastRunning(movement, deltaTime);
                return;
            }

            HandleRunning(movement, deltaTime);
        }

        private void HandleIdle(float deltaTime)
        {
            playerStateMachine.Animator.SetFloat("MovementSpeed", 0, 0.1f, deltaTime);
            Move(Vector3.zero, deltaTime);
        }

        private void HandleFastRunning(Vector3 movement, float deltaTime)
        {
            var movementSpeed = playerStateMachine.FastRunningSpeed;
            playerStateMachine.Animator.SetFloat("MovementSpeed", 2f, 0.1f, deltaTime);
            Move(movement * movementSpeed, deltaTime);
        }

        private void HandleRunning(Vector3 movement, float deltaTime)
        {
            var movementSpeed = playerStateMachine.RunningSpeed;
            playerStateMachine.Animator.SetFloat("MovementSpeed", 1f, 0.1f, deltaTime);
            Move(movement * movementSpeed, deltaTime);
        }

        private Vector3 CalculateMovement(float deltaTime)
        {
            float horizontal = playerStateMachine.InputReader.MovementValue.x;
            float vertical = playerStateMachine.InputReader.MovementValue.y;

            Vector3 move = CalculateRotation(horizontal, vertical, deltaTime);

            return move;
        }

        private Vector3 CalculateRotation(float horizontal, float vertical, float deltaTime)
        {
            float cameraAngle = playerStateMachine.MainCamera.eulerAngles.y;
            float targeAngle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + cameraAngle;
            float rotationSpeed = playerStateMachine.RotationSpeed;

            Quaternion direction = Quaternion.Euler(0, targeAngle, 0);
            Quaternion rotation = playerStateMachine.transform.rotation;

            playerStateMachine.transform.rotation = Quaternion.Slerp(rotation, direction, rotationSpeed * deltaTime);

            return direction * Vector3.forward;
        }

        private void OnTarget()
        {
            if (!playerStateMachine.Targeter.SelectTarget()) return;

            playerStateMachine.SwitchState(new PlayerTargetingState(playerStateMachine));
        }

        private void OnDodge(InputAction.CallbackContext context)
        {
            _isDodgePressed = context.ReadValueAsButton();
        }

        private void OnJump()
        {
            playerStateMachine.SwitchState(new PlayerJumpingState(playerStateMachine));
        }
    }
}