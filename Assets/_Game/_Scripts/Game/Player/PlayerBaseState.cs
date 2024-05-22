using CombatGame.Commons.StateMachine;
using UnityEngine;

namespace CombatGame.Player
{
    public abstract class PlayerBaseState : BaseState
    {
        protected PlayerController context;

        public PlayerBaseState(PlayerController playerStateMachine)
        {
            this.context = playerStateMachine;
        }

        protected void Move(float deltaTime)
        {
            Vector3 gravityApplied = context.ForceReceiver.Movement;
            context.CharacterController.Move((Vector3.zero + gravityApplied) * deltaTime);
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            Vector3 gravityApplied = context.ForceReceiver.Movement;
            context.CharacterController.Move((motion + gravityApplied) * deltaTime);
        }

        protected void FaceTarget()
        {
            if (context.Targeter.CurrentTarget == null) return;

            Vector3 targetPosition = context.Targeter.CurrentTarget.transform.position;
            Vector3 myPosition = context.transform.position;
            Vector3 lookPos = targetPosition - myPosition;
            lookPos.y = 0;

            context.transform.rotation = Quaternion.LookRotation(lookPos);
        }

        protected void Jump()
        {
            if (context.Grounded)
            {
                context.ForceReceiver.AddForce(Vector3.up * 25);
            }
        }

        protected void GroundedCheck()
        {
            Vector3 spherePosition = new(
                context.transform.position.x, 
                context.transform.position.y - context.GroundedOffset,
                context.transform.position.z);
            context.Grounded = Physics.CheckSphere(
                spherePosition, 
                context.GroundedRadius,
                context.GroundLayers,
                QueryTriggerInteraction.Ignore);
        }
    }
}