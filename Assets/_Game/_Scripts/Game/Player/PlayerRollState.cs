using UnityEngine;

namespace CombatGame.Player
{
    public class PlayerRollState : PlayerBaseState
    {
        private Vector3 _direction;

        public PlayerRollState(PlayerController stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            base.Enter();
            float horizontal = context.InputReader.MovementValue.x;
            float vertical = context.InputReader.MovementValue.y;
            float cameraAngle = 0;
            if (context.MainCamera != null)
            {
                cameraAngle = context.MainCamera.transform.eulerAngles.y;
            }
            var targetRotation = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + cameraAngle;
            context.transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f);
            _direction = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
            animator.CrossFadeInFixedTime("Roll", 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(context.Animator, tag:"Roll") >= 1)
            {
                if (context.Targeter.CurrentTarget == null)
                {
                    context.SwitchState(new PlayerGroundedState(context));
                }
                else
                {
                    context.SwitchState(new PlayerTargetingState(context));
                }
                return;
            }

            GroundedCheck();
            MovePlayer(deltaTime);
        }

        private void MovePlayer(float deltaTime)
        {
            Move(2 * context.MovementSpeed * _direction.normalized, deltaTime);
        }
    }
}