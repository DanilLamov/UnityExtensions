using System.Linq;
using System.Reflection;
using Lamov.UnityExtensions.Runtime.ColorsModule;
using Lamov.UnityExtensions.Runtime.DraggablePointModule;
using Lamov.UnityExtensions.Runtime.DraggablePointModule.Attributes;
using UnityEditor;
using UnityEngine;

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
            var draggablePointAttributeType = typeof(DraggablePointAttribute);
            var serializedObjectType = serializedObject.targetObject.GetType();

            UpdateTransformChanges();

            foreach (var field in serializedObjectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (field.GetCustomAttribute(draggablePointAttributeType, false) is not DraggablePointAttribute draggablePointAttribute) continue;
                
                var property = serializedObject.FindProperty(field.Name);
                    
                if (property.isArray)
                {
                    for (var x = 0; x < property.arraySize; x++)
                    {
                        var element = property.GetArrayElementAtIndex(x);
                        
                        if (element.propertyType == SerializedPropertyType.Vector3)
                        {
                            DrawVector3PointInArray(property.name, element, draggablePointAttribute);
                        }
                        else if (element.type == nameof(Point))
                        {
                            var point = (field.GetValue(property.serializedObject.targetObject) as Point[])[x];
                            DrawPointInArray(property.name, point, draggablePointAttribute);
                        }
                    }
                }
                else
                {
                    if (property.propertyType == SerializedPropertyType.Vector3)
                    {
                        DrawVector3Property(property, draggablePointAttribute);
                    }
                    else if (property.type == nameof(Point))
                    {
                        var point = field.GetValue(property.serializedObject.targetObject) as Point;

                        DrawPointProperty(property, point, draggablePointAttribute);
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
        
        private void DrawVector3Property(SerializedProperty property, DraggablePointAttribute draggablePointAttribute)
        {
            Handles.Label(property.vector3Value, property.name);
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();
            
            switch (draggablePointAttribute.Space)
            {
                case Space.World:
                    Handles.SphereHandleCap( 0, property.vector3Value, Quaternion.identity, .1f, EventType.Repaint);
                    property.vector3Value = Handles.PositionHandle(property.vector3Value, Quaternion.identity);
                    break;
                
                case Space.Self:
                    Handles.SphereHandleCap( 0, _targetTransform.position + property.vector3Value, Quaternion.identity, .1f, EventType.Repaint);

                    var pointWorldPosition = _targetTransform.position + property.vector3Value;
                    pointWorldPosition = _targetRotate * (pointWorldPosition - _targetTransform.position) + _targetTransform.position;

                    property.vector3Value = Handles.PositionHandle(pointWorldPosition, Quaternion.identity) - _targetTransform.position;
                    break;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawVector3PointInArray(string name, SerializedProperty property, DraggablePointAttribute draggablePointAttribute)
        {
            Handles.Label(property.vector3Value, name);
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();

            switch (draggablePointAttribute.Space)
            {
                case Space.World:
                    Handles.SphereHandleCap( 0, property.vector3Value, Quaternion.identity, .1f, EventType.Repaint);
                    property.vector3Value = Handles.PositionHandle(property.vector3Value, Quaternion.identity);
                    break;
                
                case Space.Self:
                    Handles.SphereHandleCap( 0, _targetTransform.position + property.vector3Value, Quaternion.identity, .1f, EventType.Repaint);
                    
                    var pointWorldPosition = _targetTransform.position + property.vector3Value;
                    pointWorldPosition = _targetRotate * (pointWorldPosition - _targetTransform.position) + _targetTransform.position;

                    property.vector3Value = Handles.PositionHandle(pointWorldPosition, Quaternion.identity) - _targetTransform.position;
                    break;
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPointProperty(SerializedProperty property, Point point, DraggablePointAttribute draggablePointAttribute)
        {
            point.Rotation.Normalize();
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();

            switch (draggablePointAttribute.Space)
            {
                case Space.World:
                    Handles.Label(point.Position, property.name);
                    Handles.SphereHandleCap( 0, point.Position, point.Rotation, .1f, EventType.Repaint);
                    point.Position = Handles.PositionHandle(point.Position, point.Rotation);
                    point.Rotation = Handles.RotationHandle(point.Rotation, point.Position);
                    break;
                
                case Space.Self:
                    var pointWorldPosition = _targetTransform.position + point.Position;
                    pointWorldPosition = _targetRotate * (pointWorldPosition - _targetTransform.position) + _targetTransform.position;
                    var pointRotation = point.Rotation * _targetRotate * _targetTransform.rotation;
                    
                    Handles.Label(pointWorldPosition, property.name);
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
    }
}