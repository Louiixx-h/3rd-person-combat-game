using UnityEngine;
using UnityEngine.AI;

namespace CombatGame.Commons.GamePhysics
{
    public class ForceReceiver : MonoBehaviour
    {

        [SerializeField] private CharacterController _controller;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _drag = 0.3f;

        private float _veritcalVelocity;
        private Vector3 _dampingVelocity;
        private Vector3 _impact;

        public Vector3 Movement => _impact + Vector3.up * _veritcalVelocity;

        private void Update()
        {
            if (_controller.isGrounded)
            {
                _veritcalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                _veritcalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag);
            if (_impact == Vector3.zero && _agent != null)
            {
                _agent.enabled = true;
            }
        }

        public void AddForce(Vector3 force)
        {
            _impact += force;
            if (_agent != null)
            {
                _agent.enabled = false;
            }
        }
    }
}