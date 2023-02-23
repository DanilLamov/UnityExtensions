using UnityEngine;

namespace Lamov.UnityExtensions.Runtime.Services.PoolService.PoolObjects
{
    public class PoolObject : IPoolObject
    {
        public GameObject GameObject { get; private set; }

        public PoolObject(GameObject gameObject)
        {
            GameObject = gameObject;
            GameObject.SetActive(false);
        }

        public bool IsAlive() => GameObject != null;

        public void OnExitPool() => GameObject.SetActive(true);

        public void OnEnterPool() => GameObject.SetActive(false);

        public void DestroySelf()
        {
            Object.Destroy(GameObject);
            GameObject = null;
        }
    }
}