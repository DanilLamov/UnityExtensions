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
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class DraggablePointDrawer : UnityEditor.Editor
    {
        private Transform _targetTransform;
        private Vector3 _lastTargetPosition;
        private Vector3 _targetMovement;
        private Quaternion _lastTargetRotation;
        private Quaternion _targetRotate;

        private void OnEnable()
        {
            _targetTransform = ((MonoBehaviour)target).transform;
            _lastTargetPosition = _targetTransform.position;
            _lastTargetRotation = _targetTransform.rotation;
        }

        public void OnSceneGUI()
        {
            UpdateTransformChanges();

            foreach (var (value, field, draggablePointAttribute) in GetDeepDraggablePointAttributeFields(serializedObject.targetObject))
            {
                if (value is Array array)
                {
                    foreach (var element in array)
                    {
                        switch (element)
                        {
                            case Vector3 v3:
                                DrawVector3PointInArray(field.Name, ref v3, draggablePointAttribute);
                                break;
                            case Point point:
                                DrawPointInArray(field.Name, point, draggablePointAttribute);
                                break;
                        }
                    }
                }
                else
                {
                    if (value is Vector3 v3)
                    {
                        DrawVector3Property(ref v3, field.Name, draggablePointAttribute);
                    }
                    else if (value is Point point)
                    {
                        DrawPointProperty(field.Name, point, draggablePointAttribute);
                    }
                }
            }
        }

        private void UpdateTransformChanges()
        {
            var targetPosition = _targetTransform.position;
            _targetMovement = targetPosition - _lastTargetPosition;
            _lastTargetPosition = targetPosition;
            
            var targetRotation = _targetTransform.rotation;
            _targetRotate = targetRotation * Quaternion.Inverse(_lastTargetRotation);
            _lastTargetRotation = targetRotation;
        }
        
        private void DrawVector3Property(ref Vector3 property, string name, DraggablePointAttribute draggablePointAttribute)
        {
            Handles.Label(property, name);
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();
            
            switch (draggablePointAttribute.Space)
            {
                case Space.World:
                    Handles.SphereHandleCap( 0, property, Quaternion.identity, .1f, EventType.Repaint);
                    property = Handles.PositionHandle(property, Quaternion.identity);
                    break;
                
                case Space.Self:
                    Handles.SphereHandleCap( 0, _targetTransform.position + property, Quaternion.identity, .1f, EventType.Repaint);

                    var pointWorldPosition = _targetTransform.position + property;
                    pointWorldPosition = _targetRotate * (pointWorldPosition - _targetTransform.position) + _targetTransform.position;

                    property = Handles.PositionHandle(pointWorldPosition, Quaternion.identity) - _targetTransform.position;
                    break;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawVector3PointInArray(string name, ref Vector3 property, DraggablePointAttribute draggablePointAttribute)
        {
            Handles.Label(property, name);
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();

            switch (draggablePointAttribute.Space)
            {
                case Space.World:
                    Handles.SphereHandleCap( 0, property, Quaternion.identity, .1f, EventType.Repaint);
                    property = Handles.PositionHandle(property, Quaternion.identity);
                    break;
                
                case Space.Self:
                    Handles.SphereHandleCap( 0, _targetTransform.position + property, Quaternion.identity, .1f, EventType.Repaint);
                    
                    var pointWorldPosition = _targetTransform.position + property;
                    pointWorldPosition = _targetRotate * (pointWorldPosition - _targetTransform.position) + _targetTransform.position;

                    property = Handles.PositionHandle(pointWorldPosition, Quaternion.identity) - _targetTransform.position;
                    break;
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPointProperty(string name, Point point, DraggablePointAttribute draggablePointAttribute)
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
                    var pointWorldPosition = point.TargetTransform ? point.TargetTransform.position : _targetTransform.position + point.Position;
                    pointWorldPosition = _targetRotate * (pointWorldPosition - _targetTransform.position) + _targetTransform.position;
                    var pointRotation = (point.TargetTransform ? point.TargetTransform.rotation : point.Rotation) * _targetRotate * _targetTransform.rotation;
                    
                    Handles.Label(pointWorldPosition, name);
                    Handles.SphereHandleCap( 0, pointWorldPosition, pointRotation, .1f, EventType.Repaint);
                    point.Position = Handles.PositionHandle(pointWorldPosition, pointRotation) - _targetTransform.position;
                    point.Rotation = Handles.RotationHandle(pointRotation, pointWorldPosition) * Quaternion.Inverse(_targetRotate) * Quaternion.Inverse( _targetTransform.rotation);
                    break;
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPointInArray(string name, Point point, DraggablePointAttribute draggablePointAttribute)
        {
            point.Rotation.Normalize();
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();

            switch (draggablePointAttribute.Space)
            {
                case Space.World:
                    Handles.Label(point.Position, name);
                    Handles.SphereHandleCap( 0, point.Position, point.Rotation, .1f, EventType.Repaint);
                    point.Position = Handles.PositionHandle(point.Position, point.Rotation);
                    point.Rotation = Handles.RotationHandle(point.Rotation, point.Position);
                    break;
                
                case Space.Self:
                    var pointWorldPosition = _targetTransform.position + point.Position;
                    pointWorldPosition = _targetRotate * (pointWorldPosition - _targetTransform.position) + _targetTransform.position;
                    var pointRotation = point.Rotation * _targetRotate * _targetTransform.rotation;
                    
                    Handles.Label(pointWorldPosition, name);
                    Handles.SphereHandleCap( 0, pointWorldPosition, pointRotation, .1f, EventType.Repaint);
                    point.Position = Handles.PositionHandle(pointWorldPosition, pointRotation) - _targetTransform.position;
                    point.Rotation = Handles.RotationHandle(pointRotation, pointWorldPosition) * Quaternion.Inverse(_targetRotate) * Quaternion.Inverse( _targetTransform.rotation);
                    break;
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private static IEnumerable<(Object, FieldInfo, DraggablePointAttribute)> GetDeepDraggablePointAttributeFields(Object serializedObject)
        {
            if (serializedObject == null) yield break;
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
                foreach (var x in GetDeepDraggablePointAttributeFields(fieldValue))
                {
                    yield return x;
                }
            }
        }
    }
}