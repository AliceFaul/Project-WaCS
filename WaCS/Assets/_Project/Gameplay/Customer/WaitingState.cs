using UnityEngine;

namespace _Project.Gameplay.Customer
{
    public class WaitingState : ICustomerState
    {
        private Customer _customer;
        private float _waitTimer;

        public WaitingState(Customer customer)
        {
            _customer = customer;
        }

        public void EnterState()
        {
            bool success = _customer.GetQueue().EnqueueCustomer(_customer);

            if (!success)
            {
                _customer.ChangeState(new LeavingState(_customer));
                return;
            }

            _waitTimer = 0;
        }

        public void UpdateState()
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= _customer.Brain.Patience)
            {
                _customer.GetQueue().RemoveCustomer(_customer);
                _customer.ChangeState(new LeavingState(_customer));
            }
        }

        public void ExitState() { }
    }
}
