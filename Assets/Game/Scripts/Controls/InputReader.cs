using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public bool IsAttacking { get; private set; }

    public Vector2 MovementValue {get; private set;}
    public Action JumpEvent;
    public Action TargetEvent;
    public Action<InputAction.CallbackContext> DodgeEvent;

    Controls _controls;
    
    void Start()
    {
        _controls = new Controls();
        _controls.Player.SetCallbacks(this);
        _controls.Player.Enable();
    }

    private void OnDestroy() 
    {
        _controls?.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed) return;

        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        DodgeEvent?.Invoke(context);
    }

    public void OnMove(InputAction.CallbackContext context) 
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) 
    {

    }

    public void OnTarget(InputAction.CallbackContext context) 
    {
        if(!context.performed) return;

        TargetEvent?.Invoke();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        IsAttacking = context.ReadValueAsButton();
    }
}
