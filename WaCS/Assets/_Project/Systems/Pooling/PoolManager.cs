using UnityEngine;
using System.Collections.Generic;
using System;
using _Project.Core.Singleton;

namespace _Project.Systems.Pooling
{
    public class PoolManager : PersistentSingleton<PoolManager>
    {
        private Dictionary<Type, object> _pool = new Dictionary<Type, object>();

        // Creates a new pool for the specified type and prefab. If a pool for that type already exists, it logs a warning.
        public void CreatePool<T>(T prefab, int size) where T : Component
        {
            if (_pool.ContainsKey(typeof(T)))
            {
                Debug.LogWarning($"Pool for type {typeof(T)} already exists.");
                return;
            }
            var pool = new ObjectPool<T>(prefab, size, transform);
            _pool.Add(typeof(T), pool);
        }

        // Gets an object from the pool of the specified type. If no pool exists for that type, it logs an error and returns null.
        public T Get<T>() where T : Component
        {
            if (_pool.TryGetValue(typeof(T), out var poolObj) && poolObj is ObjectPool<T> pool)
            {
                return pool.Get();
            }
            Debug.LogError($"No pool found for type {typeof(T)}. Make sure to create it first.");
            return null;
        }

        // Returns an object to the pool of the specified type. If no pool exists for that type, it logs an error.
        public void Return<T>(T obj) where T : Component
        {
            if (_pool.TryGetValue(typeof(T), out var poolObj) && poolObj is ObjectPool<T> pool)
            {
                pool.Return(obj);
            }
            else
            {
                Debug.LogError($"No pool found for type {typeof(T)}. Make sure to create it first.");
            }
        }
    }
}
