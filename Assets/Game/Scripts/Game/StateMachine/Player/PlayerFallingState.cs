using UnityEngine;

namespace Scripts.Game.StateMachine.Player
{
    public class PlayerFallingState : PlayerBaseState
    {
        private Vector3 _momentum;

        public PlayerFallingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }


        public override void Enter()
        {
            playerStateMachine.Animator.CrossFadeInFixedTime("Fall", 0.1f);

            _momentum = playerStateMachine.CharacterController.velocity;
            _momentum.y = 0;
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {
            FaceTarget();
            Move(_momentum, deltaTime);

            if (playerStateMachine.CharacterController.isGrounded)
            {
                ReturnToLocomotion();
                return;
            }
        }
    }
}