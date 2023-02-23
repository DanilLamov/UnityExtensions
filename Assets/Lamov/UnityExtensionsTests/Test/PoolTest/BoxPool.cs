using Lamov.UnityExtensions.Runtime.Services.PoolService.Data;
using Lamov.UnityExtensions.Runtime.Services.PoolService.PoolObjects;
using UnityEngine;

namespace Lamov.UnityExtensionsTests.Test.PoolTest
{
    [CreateAssetMenu(menuName = "Data/Pool/Box")]
    public class BoxPool : PoolData<PoolComponent<Box>>
    {
        [SerializeField] private Box _box;
        
        protected override PoolComponent<Box> CreatePoolEntity()
        {
            return new PoolComponent<Box>(Instantiate(_box));
        }
    }
}