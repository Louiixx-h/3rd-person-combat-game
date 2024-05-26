namespace CombatGame.Player
{
    public class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerController stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            context.InputReader.TargetEvent += HandleTarget;
            context.InputReader.JumpEvent += Jump;
            context.InputReader.RollEvent += Roll;
            animator.CrossFadeInFixedTime(context.AnimationClips[AnimationName.Grounded], 0.1f);
        }

        public override void Exit()
        {
            animator.SetFloat("MovementSpeed", 0);
            context.InputReader.TargetEvent -= HandleTarget;
            context.InputReader.JumpEvent -= Jump;
            context.InputReader.RollEvent -= Roll;
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            if (context.InputReader.IsAttacking && context.Grounded)
            {
                context.SwitchState(new PlayerAttackState(context, context.CurrentAttack));
                return;
            }

            if (!context.Grounded)
            {
                context.SwitchState(new PlayerFallState(context));
                return;
            }

            MovePlayer(deltaTime);
        }

        private void MovePlayer(float deltaTime)
        {
            var moveSpeed = context.MovementSpeed;
            if (context.InputReader.MovementValue.magnitude <= 0.2f)
            {
                animator.SetFloat("MovementSpeed", 0, 0.1f, deltaTime);
                Move(deltaTime);
                return;
            }
            
            if (context.InputReader.MovementValue.magnitude > 0.2f)
            {
                if (context.InputReader.IsDodging)
                {
                    moveSpeed *= context.RunSpeed;
                    animator.SetFloat("MovementSpeed", 1, 0.1f, deltaTime);
                }
                else
                {
                    animator.SetFloat("MovementSpeed", 0.5f, 0.1f, deltaTime);
                }
            }

            MoveAndRotate(deltaTime, moveSpeed);
        }

        private void HandleTarget()
        {
            if (!context.Targeter.SelectTarget()) return;
            context.SwitchState(new PlayerTargetingState(context));
        }

        private void Jump()
        {
            if (context.Grounded)
            {
                context.SwitchState(new PlayerJumpState(context));
            }
        }

        private void Roll()
        {
            if (context.Grounded)
            {
                context.SwitchState(new PlayerRollState(context));
            }
        }
    }
}