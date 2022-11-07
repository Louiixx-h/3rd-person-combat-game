using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        playerStateMachine.InputReader.TargetEvent += HandleTarget;
        playerStateMachine.Animator.CrossFadeInFixedTime("GroundedBlendTree", 0.1f);
    }

    public override void Exit()
    {
        playerStateMachine.Animator.SetFloat("MovementSpeed", 0);
        playerStateMachine.InputReader.TargetEvent -= HandleTarget;
    }

    public override void Tick(float deltaTime)
    {
        if(playerStateMachine.InputReader.IsAttacking)
        {
            playerStateMachine.SwitchState(new PlayerAttackState(playerStateMachine, playerStateMachine.CurrentAttack));
            return;
        }
        
        HandleMove(deltaTime);
    }

    void HandleMove(float deltaTime) 
    {
        if(playerStateMachine.InputReader.MovementValue == Vector2.zero) 
        {
            playerStateMachine.Animator.SetFloat("MovementSpeed", 0, 0.1f, deltaTime);
            Move(Vector3.zero * playerStateMachine.MovementSpeed, deltaTime);
            return;
        }
        
        Vector3 movement = CalculateMovement(deltaTime);

        Move(movement * playerStateMachine.MovementSpeed, deltaTime);
        playerStateMachine.Animator.SetFloat("MovementSpeed", 1f, 0.1f, deltaTime);
    }

    Vector3 CalculateMovement(float deltaTime) 
    {
        float horizontal = playerStateMachine.InputReader.MovementValue.x;
        float vertical = playerStateMachine.InputReader.MovementValue.y;

        Vector3 move = CalculateRotation(horizontal, vertical, deltaTime);

        return move;
    }

    Vector3 CalculateRotation(float horizontal, float vertical, float deltaTime) 
    {
        float cameraAngle = playerStateMachine.MainCamera.eulerAngles.y;
        float targeAngle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + cameraAngle;
        float rotationSpeed = playerStateMachine.RotationSpeed;
        
        Quaternion direction = Quaternion.Euler(0, targeAngle, 0);
        Quaternion rotation = playerStateMachine.transform.rotation;

        playerStateMachine.transform.rotation = Quaternion.Slerp(rotation, direction, rotationSpeed * deltaTime);

        return  direction * Vector3.forward;
    }

    void HandleTarget() {
        if(!playerStateMachine.Targeter.SelectTarget()) return;

        playerStateMachine.SwitchState(new PlayerTargetingState(playerStateMachine));
    }
}
