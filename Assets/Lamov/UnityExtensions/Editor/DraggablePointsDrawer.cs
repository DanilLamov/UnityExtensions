using System;
using System.Collections.Generic;
using System.Reflection;
using Lamov.UnityExtensions.Runtime.ColorsModule;
using Lamov.UnityExtensions.Runtime.DraggablePointModule;
using Lamov.UnityExtensions.Runtime.DraggablePointModule.Attributes;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace Lamov.UnityExtensions.Editor
{
    public class DraggablePointsDrawer
    {
        private readonly SerializedObject _serializedObject;
        private readonly Transform _targetTransform;

        public DraggablePointsDrawer(SerializedObject serializedObject)
        {
            _serializedObject = serializedObject;
            _targetTransform = ((MonoBehaviour)serializedObject.targetObject).transform;
        }
        
        public void DrawDraggablePoints()
        {
            foreach (var (value, field, draggablePointAttribute) in GetDeepDraggablePointAttributeFields(_serializedObject.targetObject))
            {
                switch (value)
                {
                    case Vector3 v3:
                        DrawVector3Property(ref v3, draggablePointAttribute, field.Name);
                        break;
                    
                    case Point point:
                        DrawPointProperty(point, draggablePointAttribute, field.Name);
                        break;
                    
                    case Array array:
                    {
                        switch (array)
                        {
                            case Vector3[] vector3Array:
                                for (var i = 0; i < vector3Array.Length; i++)
                                {
                                    DrawVector3Property(ref vector3Array[i], draggablePointAttribute, field.Name);
                                }
                                break;
                            
                            case Point[] pointsArray:
                            {
                                foreach (var point in pointsArray)
                                {
                                    DrawPointProperty(point, draggablePointAttribute, field.Name);
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            
            _serializedObject.ApplyModifiedProperties();
        }

        private static IEnumerable<(Object, FieldInfo, DraggablePointAttribute)> GetDeepDraggablePointAttributeFields(Object serializedObject, List<Object> showedObjects = null)
        {
            if (serializedObject == null) yield break;
            
            showedObjects ??= new List<object>();
            if (showedObjects.Contains(serializedObject) || (showedObjects.Count > 0 && serializedObject is MonoBehaviour)) yield break;
            showedObjects.Add(serializedObject);
            
            var serializedObjectType = serializedObject.GetType();
            if (serializedObjectType.IsPrimitive) yield break;
            
            var fields = serializedObjectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (field.GetCustomAttribute(typeof(DraggablePointAttribute), false) is DraggablePointAttribute draggablePointAttribute)
                {
                    yield return (field.GetValue(serializedObject), field, draggablePointAttribute);
                    continue;
                }

                var fieldValue = field.GetValue(serializedObject);
                foreach (var x in GetDeepDraggablePointAttributeFields(fieldValue, showedObjects))
                {
                    yield return x;
                }
            }
        }

        #region Draw

        private void DrawVector3Property(ref Vector3 property, DraggablePointAttribute draggablePointAttribute, string name)
        {
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();
            
            switch (draggablePointAttribute.Space)
            {
                case Space.World:
                    Handles.Label(property, name);
                    Handles.SphereHandleCap( 0, property, Quaternion.identity, .1f, EventType.Repaint);
                    property = Handles.PositionHandle(property, Quaternion.identity);
                    break;
                
                case Space.Self:
                    var pointWorldPosition = _targetTransform.TransformPoint(property);

                    Handles.Label(pointWorldPosition, name);
                    Handles.SphereHandleCap( 0, pointWorldPosition, Quaternion.identity, .1f, EventType.Repaint);
                    property = _targetTransform.InverseTransformPoint(Handles.PositionHandle(pointWorldPosition, Quaternion.identity));
                    break;
            }
        }

        private void DrawPointProperty(Point point, DraggablePointAttribute draggablePointAttribute, string name)
        {
            point.Rotation.Normalize();
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();

            switch (draggablePointAttribute.Space)
            {
                case Space.World:
                    if (point.TargetTransform)
                    {
                        point.Position = point.TargetTransform.position;
                        point.Rotation = point.TargetTransform.rotation;
                    }
                    
                    Handles.Label(point.Position, name);
                    Handles.SphereHandleCap( 0, point.Position, point.Rotation, .1f, EventType.Repaint);
                    point.Position = Handles.PositionHandle(point.Position, point.Rotation);
                    point.Rotation = Handles.RotationHandle(point.Rotation, point.Position);
                    break;
                
                case Space.Self:
                    var pointWorldPosition = point.TargetTransform ? point.TargetTransform.position : _targetTransform.TransformPoint(point.Position);
                    var pointWorldRotation = point.TargetTransform ? point.TargetTransform.rotation : _targetTransform.rotation * point.Rotation;
                    
                    Handles.Label(pointWorldPosition, name);
                    Handles.SphereHandleCap( 0, pointWorldPosition, pointWorldRotation, .1f, EventType.Repaint);
                    point.Position = _targetTransform.InverseTransformPoint(Handles.PositionHandle(pointWorldPosition, pointWorldRotation));
                    point.Rotation = Handles.RotationHandle(pointWorldRotation, pointWorldPosition) * Quaternion.Inverse(_targetTransform.rotation);
                    break;
            }
        }
        
        #endregion
    }
}