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
            _customer.GetCheckout().StartCheckout(_customer.name);
        }

        public void UpdateState() { }

        public void ExitState() { }
    }
}
