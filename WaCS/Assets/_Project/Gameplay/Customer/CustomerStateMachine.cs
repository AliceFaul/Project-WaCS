namespace _Project.Gameplay.Customer
{
    public class CustomerStateMachine
    {
        private ICustomerState _currentState;

        public void ChangeState(ICustomerState newState)
        {
            _currentState?.ExitState();
            _currentState = newState;
            _currentState.EnterState();
        }

        public void Update()
        {
            _currentState?.UpdateState();
        }
    }
}
