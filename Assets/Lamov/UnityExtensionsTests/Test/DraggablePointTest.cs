using Lamov.UnityExtensions.Runtime.ColorsModule;
using Lamov.UnityExtensions.Runtime.DraggablePointModule;
using Lamov.UnityExtensions.Runtime.DraggablePointModule.Attributes;
using UnityEngine;

namespace Lamov.UnityExtensionsTests.Test
{
    public class DraggablePointTest : MonoBehaviour
    {
        [SerializeField, DraggablePoint(ColorEnum.Red)] public Vector3 _point1;
        [SerializeField, DraggablePoint(ColorEnum.Green)] public Point _point2;
        
        [SerializeField, DraggablePoint(ColorEnum.Red)] public Vector3[] _points1;
        [SerializeField, DraggablePoint(ColorEnum.Black)] public Point[] _points2;
    }
}