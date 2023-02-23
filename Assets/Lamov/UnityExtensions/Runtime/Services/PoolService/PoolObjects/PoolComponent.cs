using UnityEngine;

namespace Lamov.UnityExtensions.Runtime.Services.PoolService.PoolObjects
{
    public class PoolComponent<T> : PoolObject where T : Component
    {
        public readonly T Component;
        
        public PoolComponent(T component) : base(component.gameObject)
        {
            Component = component;
        }
    }
}