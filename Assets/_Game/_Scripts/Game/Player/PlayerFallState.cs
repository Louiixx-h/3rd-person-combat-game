using UnityEngine;

namespace CombatGame.Player
{
    public class PlayerFallState : PlayerBaseState
    {

        public PlayerFallState(PlayerController stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            base.Enter();
            animator.CrossFadeInFixedTime(context.AnimationClips[AnimationName.Fall], 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            if (context.Grounded)
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

            if (context.InputReader.MovementValue.magnitude > 0.2f)
            {
                MoveAndRotate(deltaTime, context.MovementSpeed);
            }
            else 
            {
                Move(deltaTime);
            }
        }
    }
}