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
        readonly GUIStyle style = new GUIStyle();

        private void OnEnable()
        {
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;
        }

        public void OnSceneGUI()
        {
            var property = serializedObject.GetIterator();
            while (property.Next(true))
            {
                if (property.isArray)
                {
                    for (int x = 0; x < property.arraySize; x++)
                    {
                        SerializedProperty element = property.GetArrayElementAtIndex(x);
                        
                        if (element.propertyType == SerializedPropertyType.Vector3)
                        {
                            var field = serializedObject.targetObject.GetType().GetField(property.name);
                            if (field == null) continue;

                            var draggablePoints = field.GetCustomAttributes(typeof(DraggablePointAttribute), false);
                            if (draggablePoints.Length <= 0) return;
                            var draggablePointAttribute = draggablePoints[0] as DraggablePointAttribute;

                            DrawVector3PointInArray(property.name, element, draggablePointAttribute);
                        }
                        else if (element.type == nameof(Point))
                        {
                            var field = serializedObject.targetObject.GetType().GetField(property.name);
                            if (field == null) continue;

                            var point = (field.GetValue(property.serializedObject.targetObject) as Point[])[x];
                            var draggablePoints = field.GetCustomAttributes(typeof(DraggablePointAttribute), false);
                            if (draggablePoints.Length <= 0) return;
                            var draggablePointAttribute = draggablePoints[0] as DraggablePointAttribute;

                            DrawPointInArray(property.name, point, draggablePointAttribute);
                        }
                    }
                }
                else
                {
                    DrawElement(property);
                }
            }
        }

        private void DrawElement(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.Vector3)
            {
                var field = serializedObject.targetObject.GetType().GetField(property.name);
                if (field == null) return;

                var draggablePoints = field.GetCustomAttributes(typeof(DraggablePointAttribute), false);
                if (draggablePoints.Length <= 0) return;
                var draggablePointAttribute = draggablePoints[0] as DraggablePointAttribute;

                DrawVector3Property(property, draggablePointAttribute);
            }
            else if (property.type == nameof(Point))
            {
                var field = serializedObject.targetObject.GetType().GetField(property.name);
                if (field == null) return;

                var point = field.GetValue(property.serializedObject.targetObject) as Point;
                var draggablePoints = field.GetCustomAttributes(typeof(DraggablePointAttribute), false);
                if (draggablePoints.Length <= 0) return;
                var draggablePointAttribute = draggablePoints[0] as DraggablePointAttribute;

                DrawPointProperty(property, point, draggablePointAttribute);
            }
        }

        private void DrawVector3Property(SerializedProperty property, DraggablePointAttribute draggablePointAttribute)
        {
            Handles.Label(property.vector3Value, property.name);
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();
            Handles.SphereHandleCap( 0, property.vector3Value, Quaternion.identity, .1f, EventType.Repaint);
            property.vector3Value = Handles.PositionHandle(property.vector3Value, Quaternion.identity);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPointProperty(SerializedProperty property, Point point, DraggablePointAttribute draggablePointAttribute)
        {
            point.Rotation.Normalize();
            
            Handles.Label(point.Position, property.name);
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();
            Handles.SphereHandleCap( 0, point.Position, point.Rotation, .1f, EventType.Repaint);
            point.Position = Handles.PositionHandle(point.Position, point.Rotation);
            point.Rotation = Handles.RotationHandle(point.Rotation, point.Position);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawVector3PointInArray(string name, SerializedProperty property, DraggablePointAttribute draggablePointAttribute)
        {
            Handles.Label(property.vector3Value, name);
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();
            Handles.SphereHandleCap( 0, property.vector3Value, Quaternion.identity, .1f, EventType.Repaint);
            property.vector3Value = Handles.PositionHandle(property.vector3Value, Quaternion.identity);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPointInArray(string name, Point point, DraggablePointAttribute draggablePointAttribute)
        {
            point.Rotation.Normalize();
            
            Handles.Label(point.Position, name);
            Handles.color = draggablePointAttribute.ColorEnum.ToColor();
            Handles.SphereHandleCap( 0, point.Position, point.Rotation, .1f, EventType.Repaint);
            point.Position = Handles.PositionHandle(point.Position, point.Rotation);
            point.Rotation = Handles.RotationHandle(point.Rotation, point.Position);
            serializedObject.ApplyModifiedProperties();
        }
    }
}