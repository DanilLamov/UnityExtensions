using UnityEngine;

namespace Lamov.UnityExtensions.Runtime.Services.PoolService.PoolObjects
{
    public interface IPoolObject
    {
        GameObject GameObject { get; }
        
        bool IsAlive();
        void OnExitPool();
        void OnEnterPool();
        void DestroySelf();
    }
}