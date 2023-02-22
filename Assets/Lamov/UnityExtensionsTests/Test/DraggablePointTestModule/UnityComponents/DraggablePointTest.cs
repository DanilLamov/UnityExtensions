using Lamov.UnityExtensions.Runtime.ColorsModule;
using Lamov.UnityExtensions.Runtime.DraggablePointModule;
using Lamov.UnityExtensions.Runtime.DraggablePointModule.Attributes;
using Lamov.UnityExtensionsTests.Test.DraggablePointTestModule.Data;
using UnityEngine;

namespace Lamov.UnityExtensionsTests.Test.DraggablePointTestModule.UnityComponents
{
    public class DraggablePointTest : MonoBehaviour
    {
        //[SerializeField, DraggablePoint(ColorEnum.Red, Space.Self)] private Vector3 _point1;
        //[SerializeField, DraggablePoint(ColorEnum.Red, Space.Self)] private Vector3[] _points1;
        //[SerializeField, DraggablePoint(ColorEnum.Green, Space.Self)] private Point _point2;
        [SerializeField] private DraggablePointsData[] _draggablePointsData;

        
        //[SerializeField, DraggablePoint(ColorEnum.Black, Space.Self)] private Point[] _points2;

        public Vector3 GetPoint1Pos()
        {
            return Vector3.down;
        }
    }
}