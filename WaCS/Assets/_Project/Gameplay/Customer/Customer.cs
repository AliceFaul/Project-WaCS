using System;
using System.Collections.Generic;
using _Project.Systems.Game;
using UnityEngine;

namespace _Project.Gameplay.Customer
{
    [System.Serializable]
    public class CustomerProfile
    {
        public float minBudget = 20f;
        public float maxBudget = 100f;

        public float minPatience = 5f;
        public float maxPatience = 15f;

        public int minItems = 1;
        public int maxItems = 5;
    }

    public class CustomerConfig
    {
        public float Budget { get; private set; }
        public float Patience { get; private set; }

        private int _targetItemCount;

        public CustomerConfig(CustomerProfile profile)
        {
            Budget = UnityEngine.Random.Range(profile.minBudget, profile.maxBudget);
            Patience = UnityEngine.Random.Range(profile.minPatience, profile.maxPatience);
            _targetItemCount = UnityEngine.Random.Range(profile.minItems, profile.maxItems + 1);
        }

        public int TargetItemCount => _targetItemCount;

        public bool CanAfford(float price)
        {
            return Budget >= price;
        }

        public void Spend(float price)
        {
            Budget -= price;
        }
    }

    // Represents a customer's shopping cart, allowing them to add items they intend to purchase.
    public class CustomerCart
    {
        private List<ItemData> _items = new List<ItemData>();

        public IReadOnlyList<ItemData> Items => _items;

        public void Add(ItemData item)
        {
            _items.Add(item);
        }

        public float GetTotalPrice()
        {
            float total = 0;
            foreach (var item in _items)
                total += item.SellPrice;
            return total;
        }

        public void Clear()
        {
            _items.Clear();
        }
    }

    public class Customer : MonoBehaviour, IPoolable
    {
        [Header("Config")]
        [SerializeField] private CustomerProfile profile;

        [Header("References")]
        [SerializeField] private CustomerMovement movement;

        private CustomerConfig _brain;
        private CustomerCart _cart;
        private CustomerStateMachine _fsm;

        private List<ItemData> _cartItems = new List<ItemData>();

        private QueueSystem _queueSystem;
        private CheckoutSystem _checkoutSystem;
        private ShelfService _shelfService;

        private int _spawnIndex = 0;

        private Vector3 _exitPoint;

        public CustomerConfig Brain => _brain;
        public CustomerCart Cart => _cart;
        public CustomerMovement Movement => movement;
        public IReadOnlyList<ItemData> CartItems => _cartItems;

        public event Action<Customer> OnCustomerDespawned;

        public void OnSpawned()
        {
            _brain = new CustomerConfig(profile);
            _cart = new CustomerCart();
            _fsm = new CustomerStateMachine();

            _queueSystem = ServiceRegistry.Get<QueueSystem>();
            _checkoutSystem = ServiceRegistry.Get<CheckoutSystem>();
            _shelfService = ServiceRegistry.Get<ShelfService>();

            ChangeState(new ShoppingState(this));
        }

        public void Update()
        {
            _fsm?.Update();
        }

        public void ChangeState(ICustomerState state)
        {
            _fsm.ChangeState(state);
        }

        public void SetExit(Vector3 exitPos)
        {
            _exitPoint = exitPos;
        }

        public void SetExitPoint(Vector3 exitPoint)
        {
            _exitPoint = exitPoint;
        }

        public Vector3 GetExit() => _exitPoint;

        public QueueSystem GetQueue() => _queueSystem;
        public CheckoutSystem GetCheckout() => _checkoutSystem;
        public ShelfService GetShelfService() => _shelfService;

        public void PlaceItemsOnCounter(Transform counterPoint)
        {
            foreach (var item in _cartItems)
            {
                SpawnItemVisual(item, counterPoint);
            }
        }

        private void SpawnItemVisual(ItemData data, Transform counter)
        {
            var obj = Instantiate(data.Prefab);
            obj.transform.position = counter.position + new Vector3(_spawnIndex * 0.2f, 0, 0);
            _spawnIndex++;
        }

        public void OnDespawned()
        {
            _cart.Clear();
            Movement.Stop();
            _fsm = null;
            _queueSystem = null;
            _checkoutSystem = null;
            _shelfService = null;
        }

        public void Despawn()
        {
            OnCustomerDespawned?.Invoke(this);
        }
    }
}
