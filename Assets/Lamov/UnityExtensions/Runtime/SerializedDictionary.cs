using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lamov.UnityExtensions.Runtime
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] private List<TKey> _keyData = new List<TKey>();
        [SerializeField, HideInInspector] private List<TValue> _valueData = new List<TValue>();
        
        public void OnBeforeSerialize()
        {
            _keyData.Clear();
            _valueData.Clear();

            foreach (var item in this)
            {
                _keyData.Add(item.Key);
                _valueData.Add(item.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            for (var i = 0; i < _keyData.Count && i < _keyData.Count; i++)
            {
                this[_keyData[i]] = _valueData[i];
            }
        }
    }
}