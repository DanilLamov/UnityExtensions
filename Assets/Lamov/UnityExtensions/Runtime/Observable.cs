using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lamov.UnityExtensions.Runtime
{
    [Serializable]
    public class Observable<T>
    {
        [SerializeField] private T _value;
        private HashSet<Action<T>> _observers;
        
        public T Value {
            get => _value;
            set {
                if (Equals(_value, value)) return;
                _value = value;
                Notify();
            }
        }
        
        public Observable(T value = default) {
            _value = value;
            _observers = new HashSet<Action<T>>();
        }
        
        public void Subscribe(Action<T> observer, bool notifyAfterSubscribe = true) {
            _observers.Add(observer);
            
            if(!notifyAfterSubscribe) return;
            observer?.Invoke(_value);
        }

        public void Unsubscribe(Action<T> observer) {
            _observers.Remove(observer);
        }

        public void UnsubscribeAll() {
            _observers.Clear();
        }

        private void Notify() {
            foreach (var observer in _observers) {
                observer?.Invoke(_value);
            }
        }
    }
}