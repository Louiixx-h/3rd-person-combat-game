using UnityEngine;

namespace Scripts.Game.StateMachine.Player
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine playerStateMachine;

        public PlayerBaseState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
        }

        protected void Move(float deltaTime)
        {
            Vector3 gravityApplied = playerStateMachine.ForceReceiver.Movement;
            playerStateMachine.CharacterController.Move((Vector3.zero + gravityApplied) * deltaTime);
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            Vector3 gravityApplied = playerStateMachine.ForceReceiver.Movement;
            playerStateMachine.CharacterController.Move((motion + gravityApplied) * deltaTime);
        }

        protected void FaceTarget()
        {
            if (playerStateMachine.Targeter.CurrentTarget == null) return;

            Vector3 targetPosition = playerStateMachine.Targeter.CurrentTarget.transform.position;
            Vector3 myPosition = playerStateMachine.transform.position;
            Vector3 lookPos = targetPosition - myPosition;
            lookPos.y = 0;

            playerStateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
        }

        protected void ReturnToLocomotion()
        {
            if(playerStateMachine.Targeter.CurrentTarget == null)
            {
                playerStateMachine.SwitchState(new PlayerGroundedState(playerStateMachine));
            }
            else
            {
                playerStateMachine.SwitchState(new PlayerTargetingState(playerStateMachine));
            }
        }
    }
}