using UnityEngine;

namespace Lamov.UnityExtensions.Runtime
{
    public static class MathExtensions
    {
        public static Vector3 MoveToward(Vector3 current, Vector3 target, float delta)
        {
            if (current.Approximately(target)) return target;
            
            var movementDirection = target - current;
            if (movementDirection.magnitude < delta) return target;
            
            return current + movementDirection.normalized * delta;
        }

        public static bool Approximately(this Vector3 a, Vector3 b) => Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);

        public static float GetProgress(float start, float target, float value) => (value - start) / (target - start);
    }
}