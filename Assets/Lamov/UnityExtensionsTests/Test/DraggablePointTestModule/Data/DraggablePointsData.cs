using System;
using Lamov.UnityExtensions.Runtime.ColorsModule;
using Lamov.UnityExtensions.Runtime.DraggablePointModule;
using Lamov.UnityExtensions.Runtime.DraggablePointModule.Attributes;
using UnityEngine;

namespace Lamov.UnityExtensionsTests.Test.DraggablePointTestModule.Data
{
    [Serializable]
    public class DraggablePointsData
    {
        [SerializeField, DraggablePoint(ColorEnum.Green, Space.Self)] private Point _point;
        [SerializeField, DraggablePoint(ColorEnum.Green, Space.Self) ] private Point[] _points;
    }
}