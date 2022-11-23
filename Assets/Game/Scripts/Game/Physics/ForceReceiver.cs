using UnityEngine;

public class ForceReceiver : MonoBehaviour {

    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _drag = 0.3f;
    
    private float _verticalVelocity;
    private Vector3 _dampingVelocity;
    private Vector3 _impact;
    
    public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;

    private void Update() 
    {
        if(_verticalVelocity < 0 && _controller.isGrounded) 
        {
            _verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * _gravityMultiplier * Time.deltaTime;
        }

        _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag);
    }

    public void AddForce(Vector3 force)
    {
        _impact += force;
    }

    public void Jump(float jumpFoce)
    {
        _verticalVelocity += jumpFoce;
    }
}