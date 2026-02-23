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
            Debug.Log("Customer enter leaving state!");
            _customer?.Movement.MoveTo(_customer.GetExitPosition());
        }

        public void ExitState()
        {
            Debug.Log("Customer exit leaving state!");
        }

        public void UpdateState()
        {
            if(_customer == null) return;
            // Check if the customer has reached the exit point, if so, destroy the customer game object
            if (_customer.Movement.HasReachDestination)
            {
                Object.Destroy(_customer.gameObject);
            }
        }
    }
}
