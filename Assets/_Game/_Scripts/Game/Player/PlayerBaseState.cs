using CombatGame.Commons.StateMachine;
using UnityEngine;

namespace CombatGame.Player
{
    public abstract class PlayerBaseState : BaseState
    {
        protected PlayerController context;
        protected Animator animator;

        public PlayerBaseState(PlayerController playerStateMachine)
        {
            context = playerStateMachine;
            animator = context.Animator;
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            GroundedCheck();
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

        protected void MoveAndRotate(float deltaTime, float speedMultiplier = 1.0f) 
        {
            float horizontal = context.InputReader.MovementValue.x;
            float vertical = context.InputReader.MovementValue.y;
            float cameraAngle = 0;

            if (context.MainCamera != null) {
                cameraAngle = context.MainCamera.transform.eulerAngles.y;
            }

            var targetRotation = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + cameraAngle;
            float rotation = Mathf.SmoothDampAngle(
                context.transform.eulerAngles.y,
                targetRotation, 
                ref context.RotationSpeed,
                context.RotationSmoothTime
            );

            context.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            var direction = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
            Move(direction.normalized * speedMultiplier, deltaTime);
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