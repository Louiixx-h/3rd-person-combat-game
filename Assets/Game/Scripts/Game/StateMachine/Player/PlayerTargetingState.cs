using Scripts.Game.StateMachine.Player;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    public PlayerTargetingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}

    public override void Enter()
    {
        playerStateMachine.InputReader.TargetEvent += HandleCancelTargeting;
        playerStateMachine.InputReader.JumpEvent += OnJump;
        
        playerStateMachine.Animator.CrossFadeInFixedTime("TargetingBlendTree", 0.1f);
        playerStateMachine.Animator.SetLayerWeight(1, 1f);
        playerStateMachine.Animator.SetBool("IsEquippingTheWeapon", true);
    }

    public override void Exit()
    {
        playerStateMachine.InputReader.TargetEvent -= HandleCancelTargeting;
        playerStateMachine.InputReader.JumpEvent -= OnJump;

        playerStateMachine.Targeter.Cancel();
        playerStateMachine.Animator.SetLayerWeight(1, 0);
        playerStateMachine.EquippedWeapon.SaveWeapon();
    }

    public override void Tick(float deltaTime)
    {
        if(playerStateMachine.InputReader.IsAttacking)
        {
            playerStateMachine.SwitchState(new PlayerAttackState(playerStateMachine, playerStateMachine.CurrentAttack));
            return;
        }

        if(playerStateMachine.Targeter.CurrentTarget == null) 
        {
            playerStateMachine.SwitchState(new PlayerGroundedState(playerStateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * playerStateMachine.TargetingMovementSpeed, deltaTime);
        HandleAnimator(deltaTime);
        FaceTarget();
    }

    void HandleCancelTargeting()
    {
        playerStateMachine.SwitchState(new PlayerGroundedState(playerStateMachine));
    }

    void HandleAnimator(float deltaTime) 
    {
        if(playerStateMachine.InputReader.MovementValue.y == 0) 
        {
            playerStateMachine.Animator.SetFloat("TargetingForward", 0, 0.1f, deltaTime);
        }
        else 
        {
            float value = playerStateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            playerStateMachine.Animator.SetFloat("TargetingForward", value, 0.1f, deltaTime);
        }

        if(playerStateMachine.InputReader.MovementValue.x == 0) 
        {
            playerStateMachine.Animator.SetFloat("TargetingRight", 0, 0.1f, deltaTime);
        }
        else 
        {
            float value = playerStateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            playerStateMachine.Animator.SetFloat("TargetingRight", value, 0.1f, deltaTime);
        }
    }

    Vector3 CalculateMovement() {
        Vector3 movement = new Vector3();

        movement += playerStateMachine.transform.right * playerStateMachine.InputReader.MovementValue.x;
        movement += playerStateMachine.transform.forward * playerStateMachine.InputReader.MovementValue.y;
        movement.y = 0;

        return movement;
    }

    private void OnJump()
    {
        playerStateMachine.SwitchState(new PlayerJumpingState(playerStateMachine));
    }
}
