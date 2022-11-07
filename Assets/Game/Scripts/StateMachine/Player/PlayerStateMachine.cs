using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public CharacterController CharacterController {get; private set;}
    [field: SerializeField] public Animator Animator {get; private set;}
    [field: SerializeField] public InputReader InputReader {get; private set;}
    [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
    [field: SerializeField] public Targeter Targeter {get; private set;}
    [field: SerializeField] public Attack[] Attacks {get; private set;}
    [field: SerializeField] public EquippedWeapon EquippedWeapon {get; private set;}
    [field: SerializeField] public WeaponDamage WeaponDamage {get; private set;}
    [field: SerializeField] public float MovementSpeed {get; private set;}
    [field: SerializeField] public float TargetingMovementSpeed {get; private set;}
    [field: SerializeField] public float RotationSpeed {get; private set;}

    [HideInInspector] public Transform MainCamera;
    [HideInInspector] public bool IsTargeting;
    [HideInInspector] public int CurrentAttack;

    private void Start() 
    {
        CurrentAttack = 0;
        MainCamera = Camera.main.transform;
        
        EquippedWeapon.SaveWeapon();
        SwitchState(new PlayerGroundedState(this));
    }
}
