using System;
using System.Diagnostics;

namespace Lamov.UnityExtensions.Runtime
{
    public static class DebugExtensions
    {
        public static TimeSpan CalculateExecutionTime(this Action action)
        {
            var time = new Stopwatch();
            time.Start();

            action.Invoke();
            
            return time.Elapsed;
        }
    }
}