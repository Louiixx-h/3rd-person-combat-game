using UnityEngine;

namespace CombatGame.Player
{
    public class PlayerRollState : PlayerBaseState
    {
        private Vector3 _direction;

        public PlayerRollState(PlayerController stateMachine, Vector3 direction) : base(stateMachine) {
            _direction = direction;
        }

        public override void Enter()
        {
            base.Enter();
            context.Animator.CrossFadeInFixedTime("Roll", 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(context.Animator, tag:"Roll") >= 1)
            {
                context.SwitchState(new PlayerGroundedState(context));
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