using System.Collections.Generic;
using UnityEngine;

namespace _Project.Systems.Pooling
{
    // Generic object pool for any type of Component. It creates new instances as needed and manages active/inactive states.
    public class ObjectPool<T> where T : Component
    {
        private T _prefab;
        private Transform _parent;
        private Queue<T> _pool = new Queue<T>();

        // Constructor to initialize the pool with a prefab and an optional parent transform
        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;

            for(int i = 0; i < initialSize; i++)
                CreateNew();
        }

        private T CreateNew()
        {
            T newObj = GameObject.Instantiate(_prefab, _parent);
            newObj.gameObject.SetActive(false);
            _pool.Enqueue(newObj);
            return newObj;
        }

        // Gets an object from the pool
        public T Get()
        {
            if(_pool.Count == 0)
                CreateNew();
            T obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            if(obj.TryGetComponent<IPoolable>(out var poolable))
                poolable.OnSpawned();
            return obj;
        }

        // Returns an object to the pool
        public void Return(T obj)
        {
            if(obj.TryGetComponent<IPoolable>(out var poolable))
                poolable.OnDespawned();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}
