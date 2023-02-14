﻿using System;
using UnityEngine;

namespace Lamov.UnityExtensions.Runtime.DraggablePointModule
{
    [Serializable]
    public class Point
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public Point(Vector3 position = default, Quaternion rotation = default)
        {
            Position = position;
            Rotation = rotation == default ? Quaternion.identity : rotation;
        }
    }
}