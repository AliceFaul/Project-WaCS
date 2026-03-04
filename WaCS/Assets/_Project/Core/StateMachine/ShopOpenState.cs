using _Project.Gameplay.Customer;
using _Project.Systems.Game;
using UnityEngine;

public class ShopOpenState : IGameState
{
    private CustomerSpawner _customerSpawner;
    private float _timer;
    private float _spawnInterval = 10f;

    public ShopOpenState(CustomerSpawner customerSpawner, QueueSystem queueSystem, CheckoutSystem checkoutSystem)
    {
        _customerSpawner = customerSpawner;
        // Initialize the customer spawner with necessary systems
        _customerSpawner.Init(queueSystem, checkoutSystem, initialSize: 20);
    }

    public void Enter()
    {
        _timer = 0f;
    }

    public void Exit()
    {
        // Any cleanup if necessary when exiting the shop open state
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnInterval)
        {
            _customerSpawner.SpawnCustomer();
            _timer = 0f;
        }
    }
}
