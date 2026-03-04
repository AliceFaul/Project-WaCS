using _Project.Systems.Game;
using UnityEngine;

namespace _Project.Gameplay.Customer
{
    public class ShoppingState : ICustomerState
    {
        private Customer _customer;
        private ShelfService _shelfService;

        private ShelfPoint _currentPoint;
        private int _itemsCollected;

        public ShoppingState(Customer customer)
        {
            _customer = customer;
            _shelfService = customer.GetShelfService();
        }

        public void EnterState()
        {
            _itemsCollected = 0;
            GoToNextShelf();
        }

        public void UpdateState()
        {
            if (_currentPoint == null)
                return;

            if (!_customer.Movement.HasReachDestination)
                return;

            TryTakeItem();

            if (_itemsCollected >= _customer.Brain.TargetItemCount)
            {
                _customer.ChangeState(new WaitingState(_customer));
            }
            else
            {
                GoToNextShelf();
            }
        }

        public void ExitState() { }

        private void GoToNextShelf()
        {
            _currentPoint = _shelfService.GetRandomAvailablePoint();

            if (_currentPoint == null)
            {
                _customer.ChangeState(new LeavingState(_customer));
                return;
            }

            _customer.Movement.MoveTo(
                _currentPoint.CustomerStandPoint.position
            );
        }

        private void TryTakeItem()
        {
            if (_currentPoint.IsEmpty)
                return;

            var item = _currentPoint.ItemType;

            if (!_customer.Brain.CanAfford(_currentPoint.Price))
                return;

            _currentPoint.RemoveOne();

            _customer.Cart.Add(item);
            _customer.Brain.Spend(_currentPoint.Price);

            _itemsCollected++;
        }
    }
}
