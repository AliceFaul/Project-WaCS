using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using _Project.Core.Singleton;
using UnityEngine;

namespace _Project.Core.Event
{
    public class EventManager : PersistentSingleton<EventManager>, IManager
    {
        private readonly Dictionary<Type, Delegate> _eventListener = new();

        public async Task<bool> InitAsync()
        {
            _eventListener.Clear();
            await Task.CompletedTask;
            return true;
        }

        public void Register<T>(Action<T> listener)
        {
            var type = typeof(T);
            if(_eventListener.TryGetValue(type, out var existing))
            {
                _eventListener[type] = (Action<T>)existing + listener;
            }
            else
            {
                _eventListener[type] = listener;
            }
        }

        public void Unregister<T>(Action<T> listener)
        {
            var type = typeof(T);
            if(_eventListener.TryGetValue(type, out var existing))
            {
                var current = (Action<T>)existing - listener;
                if(current == null)
                    _eventListener.Remove(type);
                else
                    _eventListener[type] = current;
            }
        }

        public void Publish<T>(T eventData)
        {
            var type = typeof(T);
            if(_eventListener.TryGetValue(type, out var del))
                ((Action<T>)del)?.Invoke(eventData);
        }
    }
}