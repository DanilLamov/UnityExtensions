using System;
using System.Collections.Generic;
using System.Linq;
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
            foreach (var (value, parent, field, draggablePointAttribute) in GetDeepDraggablePointAttributeFields(_serializedObject.targetObject))
            {
                switch (value)
                {
                    case Vector3 v3:
                        DrawVector3Property(ref v3, draggablePointAttribute, field.Name);
                        field.SetValue(parent, v3);
                        break;
                    
                    case Point point:
                        DrawPointProperty(ref point, draggablePointAttribute, field.Name);
                        field.SetValue(parent, point);
                        break;
                    
                    case Vector3[] vector3Array:
                        for (var i = 0; i < vector3Array.Length; i++) DrawVector3Property(ref vector3Array[i], draggablePointAttribute, field.Name);
                        field.SetValue(parent, vector3Array);
                        break;
                            
                    case Point[] pointsArray:
                    {
                        for (var i = 0; i < pointsArray.Length; i++) DrawPointProperty(ref pointsArray[i], draggablePointAttribute, field.Name);
                        field.SetValue(parent, pointsArray);
                        break;
                    }
                }
            }

            _serializedObject.ApplyModifiedProperties();
        }

        private static IEnumerable<(Object, Object, FieldInfo, DraggablePointAttribute)> GetDeepDraggablePointAttributeFields(Object serializedObject, List<Object> showedObjects = null)
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
                var fieldValue = field.GetValue(serializedObject);
                if (fieldValue == null) continue;
                
                if (field.GetCustomAttribute(typeof(DraggablePointAttribute), false) is DraggablePointAttribute draggablePointAttribute)
                {
                    yield return (fieldValue, serializedObject, field, draggablePointAttribute);
                    continue;
                }

                if (fieldValue is Array array)
                {
                    foreach (var i in array)
                    {
                        foreach (var x in GetDeepDraggablePointAttributeFields(i, showedObjects))
                        {
                            yield return x;
                        }
                    }
                    continue;
                }
                
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
                    Handles.SphereHandleCap( 0, property, Quaternion.identity, GetPointRadius(property), EventType.Repaint);
                    property = Handles.PositionHandle(property, Quaternion.identity);
                    break;
                
                case Space.Self:
                    var pointWorldPosition = _targetTransform.TransformPoint(property);

                    Handles.Label(pointWorldPosition, name);
                    Handles.SphereHandleCap( 0, pointWorldPosition, Quaternion.identity, GetPointRadius(pointWorldPosition), EventType.Repaint);
                    property = _targetTransform.InverseTransformPoint(Handles.PositionHandle(pointWorldPosition, Quaternion.identity));
                    break;
            }
        }

        private void DrawPointProperty(ref Point point, DraggablePointAttribute draggablePointAttribute, string name)
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
                    Handles.SphereHandleCap( 0, point.Position, point.Rotation, GetPointRadius(point.Position), EventType.Repaint);
                    point.Position = Handles.PositionHandle(point.Position, point.Rotation);
                    point.Rotation = Handles.RotationHandle(point.Rotation, point.Position);
                    break;
                
                case Space.Self:
                    var pointWorldPosition = point.TargetTransform ? point.TargetTransform.position : _targetTransform.TransformPoint(point.Position);
                    var pointWorldRotation = point.TargetTransform ? point.TargetTransform.rotation : _targetTransform.rotation * point.Rotation;
                    
                    Handles.Label(pointWorldPosition, name);
                    Handles.SphereHandleCap( 0, pointWorldPosition, pointWorldRotation, GetPointRadius(pointWorldPosition), EventType.Repaint);
                    point.Position = _targetTransform.InverseTransformPoint(Handles.PositionHandle(pointWorldPosition, pointWorldRotation));
                    point.Rotation = Handles.RotationHandle(pointWorldRotation, pointWorldPosition) * Quaternion.Inverse(_targetTransform.rotation);
                    break;
            }
        }

        private static float GetPointRadius(Vector3 pointWorldPosition) => Vector3.Distance(SceneView.currentDrawingSceneView.camera.transform.position, pointWorldPosition) * .05f;

        #endregion
    }
}