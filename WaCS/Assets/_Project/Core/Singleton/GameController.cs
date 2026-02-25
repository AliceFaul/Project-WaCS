using _Project.Core.StateMachine;
using _Project.Gameplay.Customer;
using _Project.Systems.Game;
using UnityEngine;

namespace _Project.Core.Singleton
{
    public class GameController : PersistentSingleton<GameController>
    {
        private GameStateMachine _gameStateMachine;

        private QueueSystem _queueSystem;
        private CheckoutSystem _checkoutSystem;

        private void RegisterService()
        {
           _queueSystem = ServiceRegistry.Get<QueueSystem>();
           _checkoutSystem = ServiceRegistry.Get<CheckoutSystem>();
        }

        protected override void Awake()
        {
            base.Awake();
            RegisterService();
            _gameStateMachine = new GameStateMachine();
            // Initialize the first state, e.g., ShopOpenState
            try
            {
                var customerSpawner = FindAnyObjectByType<CustomerSpawner>();
                _gameStateMachine.ChangeState(new ShopOpenState(customerSpawner, _queueSystem, _checkoutSystem));
            }
            catch
            {
                Debug.LogError("CustomerSpawner not found in the scene. Please ensure it is present.");
            }
        }

        private void Update()
        {
            _gameStateMachine.Update();
        }
    }
}