namespace _Project.Gameplay.Player
{
    public class PlayerStateMachine : IStateHandler
    {
        private IState _currentState;
        
        public void ChangeState(IState newState)
        {
            if (_currentState != newState)
            {
                _currentState?.ExitState();
                _currentState = newState;
                _currentState?.EnterState();
            }
        }

        public void UpdateState()
        {
            _currentState?.TickState();
        }
    }
}