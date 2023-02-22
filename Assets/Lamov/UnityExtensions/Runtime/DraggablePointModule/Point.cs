using System;
using UnityEngine;

namespace Lamov.UnityExtensions.Runtime.DraggablePointModule
{
    [Serializable]
    public struct Point
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Transform TargetTransform => _targetTransform;
        
        [SerializeField] private Transform _targetTransform;
        
        public Point(Vector3 position = default, Quaternion rotation = default)
        {
            Position = position;
            Rotation = rotation == default ? Quaternion.identity : rotation;
            _targetTransform = null;
        }
    }
}