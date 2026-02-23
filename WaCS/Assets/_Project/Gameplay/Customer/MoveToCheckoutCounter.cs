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
            Debug.Log("Customer enter move to checkout counter state!");
            _customer.Movement.MoveTo(_customer.GetCheckoutPosition());
        }

        public void ExitState()
        {
            Debug.Log("Customer exit move to checkout counter state!");
        }

        public void UpdateState()
        {
            if(_customer == null) return;
            // Check if the customer has reached the checkout counter, if so, change the state to paying state
            if(_customer.Movement.HasReachDestination)
            {
                _customer.OnReachedCheckout();
            }
        }
    }
}
