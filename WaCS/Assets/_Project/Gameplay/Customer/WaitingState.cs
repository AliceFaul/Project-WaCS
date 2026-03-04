using UnityEngine;

namespace _Project.Gameplay.Customer
{
    public class WaitingState : ICustomerState
    {
        private readonly Customer _customer;
        private readonly float _waitTime;

        private float _timer;

        public WaitingState(Customer customer, float waitTime)
        {
            _customer = customer;
            _waitTime = waitTime;
        }

        public void EnterState()
        {
            Debug.Log("Customer enter waiting state!");
            _timer = 0f;
        }

        public void UpdateState()
        {
            _timer += Time.deltaTime;
            if(_timer >= _waitTime)
            {
                _customer.CancelQueue();
            }
        }

        public void ExitState()
        {
            Debug.Log("Customer exit waiting state!");
        }
    }
}
