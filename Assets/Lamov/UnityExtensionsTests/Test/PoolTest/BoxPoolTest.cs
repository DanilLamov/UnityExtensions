using System.Collections;
using UnityEngine;

namespace Lamov.UnityExtensionsTests.Test.PoolTest
{
    public class BoxPoolTest : MonoBehaviour
    {
        [SerializeField] private BoxPool _boxPool;

        private void Awake()
        {
            StartCoroutine(TestCoroutine());
        }

        private IEnumerator TestCoroutine()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(.1f);
                var box = _boxPool.GetElement();
                yield return new WaitForSeconds(.1f);
                _boxPool.ReturnToPool(box);
                _boxPool.GetElement();
            }
        }
    }
}