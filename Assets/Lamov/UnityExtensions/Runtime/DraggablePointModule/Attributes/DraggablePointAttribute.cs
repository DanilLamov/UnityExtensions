using System.Diagnostics;
using Lamov.UnityExtensions.Runtime.ColorsModule;
using UnityEngine;

namespace Lamov.UnityExtensions.Runtime.DraggablePointModule.Attributes
{
    [Conditional("UNITY_EDITOR")]
    public class DraggablePointAttribute : PropertyAttribute
    {
        public ColorEnum ColorEnum { get; private set; }
        public Space Space { get; private set; }

        public DraggablePointAttribute(ColorEnum colorEnum = default, Space space = Space.World)
        {
            ColorEnum = colorEnum;
            Space = space;
        }
    }
}