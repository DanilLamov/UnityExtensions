using Lamov.UnityExtensions.Editor;
using UnityEditor;
using UnityEngine;

namespace Lamov.UnityExtensionsTests.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class DraggablePointDrawer : UnityEditor.Editor
    {
        private DraggablePointsDrawer _draggablePointsDrawer;
        
        private void OnEnable()
        {
            _draggablePointsDrawer = new DraggablePointsDrawer(serializedObject);
        }

        public void OnSceneGUI()
        {
            _draggablePointsDrawer.DrawDraggablePoints();
        }
    }
}