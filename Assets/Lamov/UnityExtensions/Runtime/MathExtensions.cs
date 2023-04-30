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

        public static float MoveTowards(float current, float target, float maxDelta) => Mathf.Abs(target - current) <= maxDelta ? target : current + Mathf.Sign(target - current) * maxDelta;

        public static bool Approximately(this Vector3 a, Vector3 b) => Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);

        public static float GetProgress(float start, float target, float value) => (value - start) / (target - start);

        public static float GetProgress(Vector3 start, Vector3 target, Vector3 position)
        {
            var totalDistance = Vector3.Distance(start, target);
            var distance = Vector3.Distance(start, position);
            var progress = distance / totalDistance;
            return progress;
        }
    }
}