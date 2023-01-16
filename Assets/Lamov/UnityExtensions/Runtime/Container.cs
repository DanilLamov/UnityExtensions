using System;
using System.Collections.Generic;

namespace Lamov.UnityExtensions.Runtime
{
    public class Container
    {
        private Dictionary<Type, object> _components;

        public Container()
        {
            _components = new Dictionary<Type, object>();
        }
        
        public Container(params object[] components)
        {
            _components = new Dictionary<Type, object>();
            foreach (var component in components)
            {
                Add(component);
            }
        }

        public void Add<T>(T component) where T : class => _components.Add(component.GetType(), component);
        
        public T Get<T>() where T : class => _components[typeof(T)] as T;

        public bool TryGetComponent<T>(out T component) where T : class
        {
            if (_components.TryGetValue(typeof(T), out var componentObj))
            {
                component = componentObj as T;
                return true;
            }

            component = null;
            return false;
        }
    }
}