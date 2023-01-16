using System;
using System.Collections.Generic;

namespace Lamov.UnityExtensions.Runtime
{
    public class Container<TComponent> where TComponent : class
    {
        protected Dictionary<Type, TComponent> _components;

        public Container()
        {
            _components = new Dictionary<Type, TComponent>();
        }
        
        public Container(params TComponent[] components)
        {
            _components = new Dictionary<Type, TComponent>();
            foreach (var component in components)
            {
                Add(component);
            }
        }

        public void Add(TComponent component) => _components.Add(component.GetType(), component);
        
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