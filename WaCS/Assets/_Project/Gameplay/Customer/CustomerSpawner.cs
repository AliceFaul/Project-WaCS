using _Project.Systems.Game;
using _Project.Systems.Pooling;
using UnityEngine;

namespace _Project.Gameplay.Customer
{
    public class CustomerSpawner : MonoBehaviour
    {
        [SerializeField] private Customer customerPrefab;

        private QueueSystem _queueSystem;
        private CheckoutSystem _checkoutSystem;

        private PoolManager _poolManager;

        private void Awake()
        {
            _poolManager = PoolManager.Instance;
        }

        public void Init(QueueSystem queueSystem, CheckoutSystem checkoutSystem, int initialSize)
        {
            _queueSystem = queueSystem;
            _checkoutSystem = checkoutSystem;
            _poolManager.CreatePool(customerPrefab, initialSize);
        }

        public void SpawnCustomer()
        {
            var customer = _poolManager.Get<Customer>();
            customer.transform.position = transform.position;
            customer.SetExitPoint(transform.position);
            customer.OnCustomerDespawned += HandleCustomerDespawned;
        }

        private void HandleCustomerDespawned(Customer customer)
        {
            customer.OnCustomerDespawned -= HandleCustomerDespawned;
            _poolManager.Return(customer);
        }
    }
}
