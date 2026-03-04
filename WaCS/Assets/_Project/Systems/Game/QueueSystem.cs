using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Gameplay.Customer;
using System;
using UnityEngine;

namespace _Project.Systems.Game
{
    public class QueueSystem : IManager
    {
        private readonly Queue<Customer> _queue = new();
        public int MaxQueueLength { get; set; }

        private readonly QueuePositionCalculator _calculator;
        public int CurrentQueueLength => _queue.Count;

        public event Action<Customer> OnCustomerEnqueued;
        public event Action<Customer> OnCustomerDequeued;
        public event Action<int> QueueFull;
        public event Action<Customer, int, Vector3> OnCustomerPositionUpdated;
        public event Action<Customer> OnCustomerOnFront;

        public async Task<bool> InitAsync()
        {
            await Task.CompletedTask;
            return true;
        }

        public QueueSystem(QueuePositionCalculator calculator, int maxQueueLength)
        {
            _calculator = calculator;
            MaxQueueLength = maxQueueLength;
        }

        // Thêm các phương thức quản lý hàng đợi ở đây
        public bool EnqueueCustomer(Customer customer)
        {
            if(customer == null) return false;
            if(_queue.Contains(customer)) return false;

            if (_queue.Count >= MaxQueueLength)
            {
                QueueFull?.Invoke(MaxQueueLength);
                return false;
            }

            _queue.Enqueue(customer);
            UpdateQueuePositions();
            OnCustomerEnqueued?.Invoke(customer);
            return true;
        }

        public Customer DequeueCustomer()
        {
            if (_queue.Count == 0) return null;
            var customer = _queue.Dequeue();
            UpdateQueuePositions();
            OnCustomerDequeued?.Invoke(customer);
            return customer;
        }

        public Customer Peek()
        {
            if (_queue.Count == 0) return null;
            return _queue.Peek();
        }

        public bool IsCustomerInQueue(Customer customer)
        {
            return _queue.Contains(customer);
        }

        private void UpdateQueuePositions()
        {
            int index = 0;
            foreach (var customer in _queue)
            {
                var targetPosition = _calculator.GetQueuePosition(index);
                OnCustomerPositionUpdated?.Invoke(customer, index, targetPosition);
                if(index == 0)
                    OnCustomerOnFront?.Invoke(customer);
                index++;
            }
        }

        public bool RemoveCustomer(Customer customer)
        {
            if(!_queue.Contains(customer)) return false;
            var tempQueue = new List<Customer>(_queue);
            tempQueue.Remove(customer);
            _queue.Clear();
            foreach(var c in tempQueue)
                _queue.Enqueue(c);
            UpdateQueuePositions();
            OnCustomerDequeued?.Invoke(customer);
            return true;
        }

        public int GetQueueLength()
        {
            return _queue.Count;
        }
    }
}
