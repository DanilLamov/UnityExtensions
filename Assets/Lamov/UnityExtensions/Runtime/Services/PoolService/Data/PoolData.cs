using Lamov.UnityExtensions.Runtime.Services.PoolService.PoolObjects;
using UnityEngine;

namespace Lamov.UnityExtensions.Runtime.Services.PoolService.Data
{
    public abstract class PoolData<T> : ScriptableObject where T : IPoolObject
    {
        [SerializeField, Min(1)] private int _poolIncreaseSize = 1;
        
        private PoolContainer _poolContainer;
        
        public void SetupPool()
        {
            _poolContainer = new GameObject($"Pool {name}").AddComponent<PoolContainer>();
            _poolContainer.Initialize(() => CreatePoolEntity(), _poolIncreaseSize);
        }
        
        public T GetElement()
        {
            if (_poolContainer == null) SetupPool();
            return (T)_poolContainer.Get();
        }

        public void ReturnToPool(T element)
        {
            if (_poolContainer == null) return;
            
            if (element == null)
            {
                Debug.LogWarning("Pool element is null");
                return;
            }
            
            _poolContainer.Put(element);
        }
        
        protected abstract T CreatePoolEntity();
    }
}