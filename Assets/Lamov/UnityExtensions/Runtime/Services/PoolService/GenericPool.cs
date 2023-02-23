using System;
using System.Collections.Generic;
using Lamov.UnityExtensions.Runtime.Services.PoolService.PoolObjects;

namespace Lamov.UnityExtensions.Runtime.Services.PoolService
{
    public class GenericPool<T> where T : IPoolObject
    {
        public bool IsEmpty => _pool.Count == 0;
        public int Count => _pool.Count;
        
        private readonly Queue<T> _pool;
        private readonly Func<T> _createObjectFunction;
        private readonly int _increaseSize;

        public GenericPool(Func<T> createObjectFunction, int increaseSize = 1)
        {
            _createObjectFunction = createObjectFunction;
            _increaseSize = increaseSize;
            _pool = new Queue<T>();
        }
        
        public void Put(T poolObject)
        {
            poolObject.OnEnterPool();
            _pool.Enqueue(poolObject);
        }

        public T Get()
        {
            if (IsEmpty) PopulatePool(_increaseSize);
            
            var poolObject = _pool.Dequeue();
            poolObject.OnExitPool();
            return poolObject;
        }
        
        public void PopulatePool(int count)
        {
            for (var i = 0; i < count; i++) Put(_createObjectFunction());
        }
    }
}