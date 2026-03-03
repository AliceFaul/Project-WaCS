using UnityEngine;

namespace _Project.Gameplay.Customer
{
    public class PayingState : ICustomerState
    {
        private Customer _customer;

        public PayingState(Customer customer)
        {
            _customer = customer;
        }

        public void EnterState()
        {
            Debug.Log("Customer enter paying state!");
            _customer.StartCheckout();
        }

        public void ExitState()
        {
            Debug.Log("Customer exit paying state!");
        }

        public void UpdateState()
        {

        }
    }
}
