using UnityEngine;

namespace Lamov.UnityExtensions.Runtime.Draw
{
    public static class GizmosDrawExtensions
    {
        public static void DrawCircle(Vector3 position, Vector3 up, float radius = 1.0f)
        {
	        up = up.normalized * radius;
            var forward = Vector3.Slerp(up, -up, 0.5f);
            var right = Vector3.Cross(up, forward).normalized * radius;
		
            var matrix = new Matrix4x4
            {
	            [0] = right.x,
	            [1] = right.y,
	            [2] = right.z,
	            [4] = up.x,
	            [5] = up.y,
	            [6] = up.z,
	            [8] = forward.x,
	            [9] = forward.y,
	            [10] = forward.z
            };

            var lastPoint = position + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)));
            var nextPoint = Vector3.zero;

            for(var i = 0; i < 91; i++){
	            nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
	            nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
	            nextPoint.y = 0;
			
	            nextPoint = position + matrix.MultiplyPoint3x4(nextPoint);
			
                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }
        }

        public static void DrawRect(Vector3 position, Vector3 up, Vector3 forward, float length = 1, float width = 2)
        {
	        up.Normalize();
	        forward.Normalize();
	        var right = Vector3.Cross(up, forward).normalized;

	        var topLeft = position + forward * (width * .5f) - right * (length * .5f);
	        var topRight = position + forward * (width * .5f) + right * (length * .5f);
	        var bottomLeft = position - forward * (width * .5f) - right * (length * .5f);
	        var bottomRight = position - forward * (width * .5f) + right * (length * .5f);

	        DrawCurve(new []{ topLeft, topRight, bottomRight, bottomLeft }, true);
        }

        public static void DrawBone(Vector3 start, Vector3 end, Vector3 forward)
        {
	        forward.Normalize();
	        var direction = end - start;
	        var right = Vector3.Cross(direction.normalized, forward).normalized;

	        var sideSize = direction.magnitude * .1f;
	        
	        var topLeft = start + forward * (sideSize * .5f) - right * (sideSize * .5f);
	        var topRight = start + forward * (sideSize * .5f) + right * (sideSize * .5f);
	        var bottomLeft = start - forward * (sideSize * .5f) - right * (sideSize * .5f);
	        var bottomRight = start - forward * (sideSize * .5f) + right * (sideSize * .5f);
	        
	        DrawCurve(new []{ topLeft, topRight, bottomRight, bottomLeft }, true);
	        
	        Gizmos.DrawLine(topLeft, end);
	        Gizmos.DrawLine(topRight, end);
	        Gizmos.DrawLine(bottomLeft, end);
	        Gizmos.DrawLine(bottomRight, end);
        }

        public static void DrawBones(Vector3[] points)
        {
	        var previousPreviousPosition = points[0];
	        var previousPosition = points[1];

	        var newBoneDirection = (previousPosition - previousPreviousPosition).normalized;
	        var right = Vector3.Cross((points[2] - previousPosition).normalized, newBoneDirection).normalized;
	        var forward = Vector3.Cross(newBoneDirection, right);
	        DrawBone(previousPreviousPosition, previousPosition, forward);

	        for (var i = 2; i < points.Length; i++)
	        {
		        newBoneDirection = (points[i] - previousPosition).normalized;
		        right = Vector3.Cross((previousPreviousPosition - previousPosition).normalized, newBoneDirection).normalized;
		        forward = Vector3.Cross(newBoneDirection, right);
		        DrawBone(previousPosition, points[i], forward);

		        previousPreviousPosition = previousPosition;
		        previousPosition = points[i];
	        }
        }
        
        public static void DrawCurve(Vector3[] points, bool isClosed = false)
        {
	        var lastPoint = points[0];
	        if (isClosed) Gizmos.DrawLine(lastPoint, points[^1]);

	        for (var i = 1; i < points.Length; i++)
	        {
		        Gizmos.DrawLine(lastPoint, points[i]);
		        lastPoint = points[i];
	        }
        }
    }
}