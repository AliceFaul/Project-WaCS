using _Project.Systems.Game;
using UnityEngine;

namespace _Project.Gameplay.Customer
{
    public class Customer : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private CustomerMovement movement;
        [SerializeField] private float waitTime;
        [SerializeField] private Transform[] shoppingPoints;
        [SerializeField] private float shoppingDuration = 5f;

        private QueueSystem _queueSystem;
        private CheckoutSystem _checkoutSystem;

        private CustomerStateMachine _stateMachine;
        private bool _isInQueue;

        public CustomerMovement Movement => movement;
        public Transform[] ShoppingPoints => shoppingPoints;

        public void Init(QueueSystem queueSystem, CheckoutSystem checkoutSystem)
        {
            _queueSystem = queueSystem;
            _checkoutSystem = checkoutSystem;
            _stateMachine = new CustomerStateMachine();
            SubscribeQueueEvent();

            EnterShoppingState();
        }

        private void Update()
        {
            _stateMachine?.Update();
        }

        public void StartCheckout()
        {
            _checkoutSystem.StartCheckout(gameObject.name);
        }

        public void GoToQueue()
        {
            if(_queueSystem.EnqueueCustomer(this))
            {
                _isInQueue = true;
                EnterWaitingState();
            }
            else
            {
                EnterLeavingState();
            }
        }

        public void CancelQueue()
        {
            if(!_isInQueue) return;
            _isInQueue = false;
            _queueSystem.RemoveCustomer(this);
            EnterLeavingState();
        }

        private void SubscribeQueueEvent()
        {
            if(_queueSystem != null)
            {
                _queueSystem.OnCustomerPositionUpdated += HandleQueuePositionChanged;
                _queueSystem.OnCustomerOnFront += HandleReachedFront;
                _queueSystem.OnCustomerDequeued += HandleDequeued;
            }
        }

        private void OnDestroy()
        {
            if(_queueSystem != null)
            {
                _queueSystem.OnCustomerPositionUpdated -= HandleQueuePositionChanged;
                _queueSystem.OnCustomerOnFront -= HandleReachedFront;
                _queueSystem.OnCustomerDequeued -= HandleDequeued;
            }
        }

        #region Customer State Handler
        private void EnterWaitingState()
        {
            _stateMachine.ChangeState(new WaitingState(this, waitTime));
        }

        private void EnterLeavingState()
        {
            _stateMachine.ChangeState(new LeavingState(this));
        }

        private void EnterPayingState()
        {
            _stateMachine.ChangeState(new PayingState(this));
        }

        private void EnterMoveToCheckoutCounterState()
        {
            _stateMachine.ChangeState(new MoveToCheckoutCounter(this));
        }

        private void EnterShoppingState()
        {
            _stateMachine.ChangeState(new ShoppingState(this, shoppingDuration));
        }
        #endregion

        #region Queue Events
        private void HandleQueuePositionChanged(Customer customer, int index, Vector3 position)
        {
            if (customer != this) return;
            movement.MoveTo(position);
        }

        private void HandleReachedFront(Customer customer)
        {
            if (customer != this) return;
            EnterMoveToCheckoutCounterState();
        }

        private void HandleDequeued(Customer customer)
        {
            if (customer != this) return;
            _isInQueue = false;
            EnterLeavingState();
        }
        #endregion

        #region Utilities
        public Vector3 GetCheckoutPosition()
        {
            return _checkoutSystem.GetCheckoutPosition();
        }

        public void OnReachedCheckout()
        {
            // Handle actions when the customer reaches the checkout, e.g., start scanning items, show UI, etc.
            EnterPayingState();
        }

        public Vector3 GetRandomShoppingPoint()
        {
            if(shoppingPoints == null || shoppingPoints.Length == 0)
                return transform.position;
            int index = Random.Range(0, shoppingPoints.Length);
            return shoppingPoints[index].position;
        }

        public Vector3 GetExitPosition()
        {
            // Return the position where the customer should move to when leaving
            return Vector3.zero; // Placeholder, replace with actual exit position
        }

        // TODO: enum reason define why customer leave (queue full, no shopping point, etc.)
        public void OnPaymentCompleted()
        {
            // Handle actions after payment is completed, e.g., show thank you message, play animation, etc.
            if(!_isInQueue) return;
            _queueSystem.DequeueCustomer();
        }
        #endregion
    }
}