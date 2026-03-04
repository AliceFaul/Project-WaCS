using UnityEngine;

namespace _Project.Gameplay.Customer
{
    public class MoveToCheckoutCounter : ICustomerState
    {
        private Customer _customer;

        public MoveToCheckoutCounter(Customer customer)
        {
            _customer = customer;
        }

        public void EnterState()
        {
            var pos = _customer.GetCheckout().GetCheckoutPosition();
            _customer.Movement.MoveTo(pos);
        }

        public void UpdateState()
        {
            if (_customer.Movement.HasReachDestination)
            {
                _customer.ChangeState(new PayingState(_customer));
            }
        }

        public void ExitState() { }
    }
}
