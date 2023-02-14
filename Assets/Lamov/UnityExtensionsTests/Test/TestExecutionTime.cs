using System;
using System.Collections.Generic;
using Lamov.UnityExtensions.Runtime;
using UnityEngine;

namespace Lamov.UnityExtensionsTests.Test
{
    [ExecuteAlways]
    public class TestExecutionTime : MonoBehaviour
    {
        private void Update()
        {
            var time = DebugExtensions.CalculateExecutionTime(TestMethod);
            Debug.Log($"t: {time.Milliseconds}");
        }

        private void TestMethod()
        {
            var list = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                list.Add(i);
            }

            var message = "";
            for (int i = 0; i < 10000; i++)
            {
                message += i;
            }
        }
    }
}