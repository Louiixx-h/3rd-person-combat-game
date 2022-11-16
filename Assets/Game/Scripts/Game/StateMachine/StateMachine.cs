using UnityEngine;

namespace Scripts.Game.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        State _currentState;

        public void SwitchState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        private void Update()
        {
            _currentState?.Tick(Time.deltaTime);
        }
    }
}