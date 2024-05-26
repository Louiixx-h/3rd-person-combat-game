using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public bool IsAttacking { get; private set; }
    public bool IsDodging { get; private set; }
    public bool IsBlocking { get; private set; }
    public Vector2 MovementValue {get; private set;}
    public Action JumpEvent;
    public Action RollEvent;
    public Action TargetEvent;

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

    public void OnLook(InputAction.CallbackContext context) { }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed) return;

        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context) 
    {
        IsDodging = context.ReadValueAsButton();
    }

    public void OnMove(InputAction.CallbackContext context) 
    {
        MovementValue = context.ReadValue<Vector2>();
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

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        RollEvent?.Invoke();
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        IsBlocking = context.ReadValueAsButton();
    }
}
