using _Project.Systems.Game;
using UnityEngine;

namespace _Project.Gameplay.Customer
{
    public enum ShoppingPhase
    {
        MoveToShelf,
        Browse
    }

    public class ShoppingState : ICustomerState
    {
        private Customer _customer;
        private Transform _currentShelf;

        private float _totalShoppingTime;
        private float _shoppingTimer;

        private float _browseTimer;
        private float _currentBrowseTime;

        private int _shelvesToVisit;
        private int _shelvesVisited;

        private ShoppingPhase _phase;

        // TODO: This should be generated based on the customer's shopping list and the shelves they visit
        private CustomerCart _cart;

        public ShoppingState(Customer customer, float totalShoppingTime)
        {
            _customer = customer;
            _totalShoppingTime = totalShoppingTime;
            _cart = _customer.CustomerCart;
        }

        public void EnterState()
        {
            _shoppingTimer = 0f;
            _shelvesVisited = 0;
            _shelvesToVisit = _customer.ShoppingPoints.Length;
            if(_shelvesToVisit == 0)
            {
                Debug.LogWarning("Customer has no shopping points");
                FinishShopping();
                return;
            }
            GoToNextShelf();
        }

        public void ExitState()
        {
            Debug.Log("Customer exit shopping state!");
        }

        public void UpdateState()
        {
            _shoppingTimer += Time.deltaTime;
            if (_shoppingTimer >= _totalShoppingTime)
            {
                FinishShopping();
                return;
            }
            switch (_phase)
            {
                case ShoppingPhase.MoveToShelf:
                    HandleMoving();
                    break;
                case ShoppingPhase.Browse:
                    HandleBrowsing();
                    break;
            }
        }

        // Phase: Moving to Shelf
        private void HandleMoving()
        {
            if (_customer.Movement.HasReachDestination)
            {
                StartBrowsing();
            }
        }

        private void GoToNextShelf()
        {
            _currentShelf = _customer.GetRandomShoppingPoint();
            _customer.Movement.MoveTo(_currentShelf.position);
            _phase = ShoppingPhase.MoveToShelf;
        }

        private void TakeItemFromShelf()
        {
            
        }

        private Transform GetClosestShelf()
        {
            Transform closestShelf = null;
            float closestDistance = Mathf.Infinity;
            Vector3 currentPosition = _customer.transform.position;
            foreach (var shelf in _customer.ShoppingPoints)
            {
                float distance = Vector3.Distance(currentPosition, shelf.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestShelf = shelf;
                }
            }
            return closestShelf;
        }

        // Phase: Browsing
        private void StartBrowsing()
        {
            _currentBrowseTime = Random.Range(1f, 2f);
            _browseTimer = 0f;
            _phase = ShoppingPhase.Browse;
        }

        private void HandleBrowsing()
        {
            _browseTimer += Time.deltaTime;
            if (_browseTimer >= _currentBrowseTime)
            {
                TakeItemFromShelf();
                _shelvesVisited++;
                if (_shelvesVisited >= _shelvesToVisit)
                {
                    // Finish shopping, go to queue
                    FinishShopping();
                }
                else
                {
                    GoToNextShelf();
                }
            }
        }

        private void FinishShopping()
        {
            // TODO: Generate Cart
            _customer.GoToQueue();
        }
    }
}
