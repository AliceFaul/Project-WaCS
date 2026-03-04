using UnityEngine;

namespace _Project.Gameplay.Customer
{
    public class LeavingState : ICustomerState
    {
        private Customer _customer;

        public LeavingState(Customer customer)
        {
            _customer = customer;
        }

        public void EnterState()
        {
            _customer.Movement.MoveTo(_customer.GetExit());
        }

        public void UpdateState()
        {
            if (_customer.Movement.HasReachDestination)
            {
                _customer.Despawn();
            }
        }

        public void ExitState() { }
    }
}
