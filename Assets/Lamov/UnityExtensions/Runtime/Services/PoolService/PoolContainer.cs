using System;
using Lamov.UnityExtensions.Runtime.Services.PoolService.PoolObjects;
using UnityEngine;

namespace Lamov.UnityExtensions.Runtime.Services.PoolService
{
    public class PoolContainer : MonoBehaviour
    {
        private GenericPool<IPoolObject> _pool;
        private Func<IPoolObject> _createPoolObjectFunction;
        
        public bool IsEmpty => _pool.IsEmpty;
        public int Allocated { get; private set; }
        public int Available => _pool.Count;
        
        public void Initialize(Func<IPoolObject> createPoolObjectFunction, int _poolIncreaseSize)
        {
            _createPoolObjectFunction = createPoolObjectFunction;

            _pool = new GenericPool<IPoolObject>(CreatePoolObject, _poolIncreaseSize);
            _pool.PopulatePool(_poolIncreaseSize);

            Allocated = 0;
        }
        
        public IPoolObject Get() => _pool.Get();

        public void Put(IPoolObject poolObject)
        {
            if (poolObject.IsAlive())
            {
                _pool.Put(poolObject);
            }
            else
            {
                poolObject.DestroySelf();
                Allocated--;
            }
        }
        
        private IPoolObject CreatePoolObject()
        {
            Allocated++;
            IPoolObject poolObject = _createPoolObjectFunction();
            poolObject.GameObject.transform.SetParent(transform);
            return poolObject;
        }
    }
}