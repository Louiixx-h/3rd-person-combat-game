using UnityEngine;

public class ForceReceiver : MonoBehaviour {

    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _drag = 0.3f;
    
    private float _veritcalVelocity;
    private Vector3 _dampingVelocity;
    private Vector3 _impact;
    
    public Vector3 Movement => _impact + Vector3.up * _veritcalVelocity;

    private void Update() 
    {
        if(_controller.isGrounded) 
        {
            _veritcalVelocity = Physics.gravity.y;
        }
        else
        {
            _veritcalVelocity += Physics.gravity.y;
        }

        _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag);
    }

    public void AddForce(Vector3 force)
    {
        _impact += force;
    }
}