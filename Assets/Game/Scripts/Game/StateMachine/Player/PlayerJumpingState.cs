using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Game.StateMachine.Player
{
    public class PlayerJumpingState : PlayerBaseState
    {
        private Vector3 _momentum;

        public PlayerJumpingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        public override void Enter()
        {
            playerStateMachine.Animator.CrossFadeInFixedTime("Jump", 0.1f);
            playerStateMachine.ForceReceiver.Jump(playerStateMachine.JumpForce);
            
            _momentum = playerStateMachine.CharacterController.velocity;
            _momentum.y = 0;
        }

        public override void Exit()
        {
            
        }

        public override void Tick(float deltaTime)
        {
            Move(_momentum, deltaTime);

            if(-playerStateMachine.CharacterController.velocity.y <= 0) 
            {
                playerStateMachine.SwitchState(new PlayerFallingState(playerStateMachine));
                return;
            }

            FaceTarget();
        }
    }
}
