using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using _Project.Core.Singleton;
using UnityEngine;

namespace _Project.Core.Event
{
    public class EventManager : PersistentSingleton<EventManager>, IManager
    {
        private readonly Dictionary<string, Action> _eventListeners =  new Dictionary<string, Action>();
        
        public async Task<bool> InitAsync()
        {
            await Task.CompletedTask;
            return true;
        }
        
        public void Register(string eventName, Action listener)
        {
            _eventListeners.TryAdd(eventName, null);
            _eventListeners[eventName] += listener;
            Debug.Log($"[EventManager] registered {eventName}");
        }

        public void Unregister(string eventName, Action listener)
        {
            if (_eventListeners.ContainsKey(eventName))
            {
                _eventListeners[eventName] -= listener;
            }
            Debug.Log("[EventManager] unregistered " + eventName);
        }

        public void Trigger(string eventName)
        {
            if (_eventListeners.TryGetValue(eventName, out var listener))
            {
                listener?.Invoke();
            }
            Debug.Log($"[EventManager] triggered {eventName}");
        }
    }
}