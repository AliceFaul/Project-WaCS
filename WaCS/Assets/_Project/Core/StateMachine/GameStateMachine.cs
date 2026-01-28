namespace _Project.Core.StateMachine
{
    public class GameStateMachine
    {
        private IGameState _currentState;

        public void ChangeState(IGameState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public void Update()
        {
            _currentState?.Tick();
        }
    }
}