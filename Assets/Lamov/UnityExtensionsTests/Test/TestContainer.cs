using Lamov.UnityExtensions.Runtime;
using UnityEngine;

namespace Lamov.UnityExtensionsTests.Test
{
    public class TestContainer : MonoBehaviour
    {
        private void Awake()
        {
            var container = new Container<object>(new A(), new B());
        }
    }

    public class A
    {
        
    }

    public class B
    {
        
    }
}