namespace CombatGame.Player.States {
    public class PlayerBlockState : PlayerBaseState {
        
        private bool _isBlockState = false;

        public PlayerBlockState(PlayerController stateMachine) : base(stateMachine) { }

        public override void Enter() {
            base.Enter();
            animator.CrossFadeInFixedTime("Block", 0.1f);
        }

        public override void Tick(float deltaTime) {
            base.Tick(deltaTime);
            
            if (!context.InputReader.IsBlocking) {
                context.SwitchState(new PlayerGroundedState(context));
                return;
            }

            if (context.InputReader.IsAttacking) {
                context.SwitchState(new PlayerAttackState(context, context.CurrentAttack));
                return;
            }

            if (!context.Grounded) {
                context.SwitchState(new PlayerFallState(context));
                return;
            }

            HandleBlock();
        }

        private void HandleBlock()
        {
            if (context.InputReader.IsBlocking)
            {
                if (context.InputReader.MovementValue.magnitude <= 0.2f)
                {
                    animator.SetLayerWeight(1, 0);
                }
                else
                {
                    animator.SetLayerWeight(0, 1);
                }
                if (!_isBlockState) {
                    context.ShieldSwitcher.Use();
                    _isBlockState = true;
                    animator.CrossFadeInFixedTime("block", 0.1f, 2);
                }
            }
            else 
            {
                if (_isBlockState)
                {
                    context.ShieldSwitcher.Save();
                    _isBlockState = false;
                }
            }

            animator.SetBool("blockState", _isBlockState);
        }
    }
}