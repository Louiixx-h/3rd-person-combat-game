using UnityEngine;

namespace CombatGame.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerController stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            base.Enter();
            animator.CrossFadeInFixedTime(context.AnimationClips[AnimationName.Jump], 0.1f);
            context.ForceReceiver.AddForce(Vector3.up * context.JumpForce);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName(context.AnimationClips[AnimationName.Jump]) 
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
            {
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
                }
                else
                {
                    context.SwitchState(new PlayerFallState(context));
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