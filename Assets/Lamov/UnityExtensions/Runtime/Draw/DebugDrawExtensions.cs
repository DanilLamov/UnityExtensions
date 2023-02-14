using UnityEngine;

namespace Lamov.UnityExtensions.Runtime.Draw
{
    public static class DebugDrawExtensions
    {
        public static void DrawCircle(Vector3 position, Vector3 up, float radius = 1.0f, Color color = default, float duration = 0, bool depthTest = true)
        {
            up = up.normalized * radius;
            var forward = Vector3.Slerp(up, -up, .5f);
            var right = Vector3.Cross(up, forward).normalized*radius;
		
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
		
            color = color == default ? Color.white : color;
		
            for(var i = 0; i < 91; i++){
                nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
                nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
                nextPoint.y = 0f;
			
                nextPoint = position + matrix.MultiplyPoint3x4(nextPoint);
			
                Debug.DrawLine(lastPoint, nextPoint, color, duration, depthTest);
                lastPoint = nextPoint;
            }
        }

        public static void DrawRect(Vector3 position, Vector3 up, Vector3 forward, float length = 1, float width = 2, Color color = default, float duration = 0, bool depthTest = true)
        {
	        up.Normalize();
	        forward.Normalize();
	        var right = Vector3.Cross(up, forward).normalized;
	        
	        color = color == default ? Color.white : color;

	        var topLeft = position + forward * (width * .5f) - right * (length * .5f);
	        var topRight = position + forward * (width * .5f) + right * (length * .5f);
	        var bottomLeft = position - forward * (width * .5f) - right * (length * .5f);
	        var bottomRight = position - forward * (width * .5f) + right * (length * .5f);

	        DrawCurve(new []{ topLeft, topRight, bottomRight, bottomLeft }, true, color, duration, depthTest);
        }
        
        public static void DrawCurve(Vector3[] points, bool isClosed = false, Color color = default, float duration = 1f/ 30f, bool depthTest = true)
        {
	        color = color == default ? Color.white : color;
	        
	        var lastPoint = points[0];
	        if (isClosed) Debug.DrawLine(lastPoint, points[^1], color, duration, depthTest);
	        
	        for (var i = 1; i < points.Length; i++)
	        {
		        Debug.DrawLine(lastPoint, points[i], color, duration, depthTest);
		        lastPoint = points[i];
	        }
        }
        
        public static void DrawBone(Vector3 start, Vector3 end, Vector3 forward, Color color = default, float duration = 1f/ 30f, bool depthTest = true)
        {
	        color = color == default ? Color.white : color;
	        
	        forward.Normalize();
	        var direction = end - start;
	        var right = Vector3.Cross(direction.normalized, forward).normalized;

	        var sideSize = direction.magnitude * .1f;
	        
	        var topLeft = start + forward * (sideSize * .5f) - right * (sideSize * .5f);
	        var topRight = start + forward * (sideSize * .5f) + right * (sideSize * .5f);
	        var bottomLeft = start - forward * (sideSize * .5f) - right * (sideSize * .5f);
	        var bottomRight = start - forward * (sideSize * .5f) + right * (sideSize * .5f);
	        
	        DrawCurve(new []{ topLeft, topRight, bottomRight, bottomLeft }, true, color, duration, depthTest);
	        
	        Debug.DrawLine(topLeft, end, color, duration, depthTest);
	        Debug.DrawLine(topRight, end, color, duration, depthTest);
	        Debug.DrawLine(bottomLeft, end, color, duration, depthTest);
	        Debug.DrawLine(bottomRight, end, color, duration, depthTest);
        }
        
        public static void DrawBones(Vector3[] points, Color color = default, float duration = 1f/ 30f, bool depthTest = true)
        {
	        var previousPreviousPosition = points[0];
	        var previousPosition = points[1];

	        var newBoneDirection = (previousPosition - previousPreviousPosition).normalized;
	        var right = Vector3.Cross((points[2] - previousPosition).normalized, newBoneDirection).normalized;
	        var forward = Vector3.Cross(newBoneDirection, right);
	        DrawBone(previousPreviousPosition, previousPosition, forward, color, duration, depthTest);

	        for (var i = 2; i < points.Length; i++)
	        {
		        newBoneDirection = (points[i] - previousPosition).normalized;
		        right = Vector3.Cross((previousPreviousPosition - previousPosition).normalized, newBoneDirection).normalized;
		        forward = Vector3.Cross(newBoneDirection, right);
		        DrawBone(previousPosition, points[i], forward, color, duration, depthTest);

		        previousPreviousPosition = previousPosition;
		        previousPosition = points[i];
	        }
        }
    }
}