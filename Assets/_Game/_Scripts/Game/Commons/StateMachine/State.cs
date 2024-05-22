namespace CombatGame.Commons.StateMachine
{
    public interface State
    {
        public void Enter();
        public void Tick(float deltaTime);
        public void Exit();
    }
}