using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.PKGameCore.PKData
{
    public class DataWrapperDictionary<TK, TV> : DataWrapperBase<Dictionary<TK, DataWrapper<TV>>>
    {
        public DataWrapperDictionary()
        {
            _data = new Dictionary<TK, DataWrapper<TV>>();
        }

        public DataWrapperDictionary(Dictionary<TK, DataWrapper<TV>> dic)
        {
            _data = dic;
        }

        public TV GetData(TK key)
        {
            bool valExist = _data.TryGetValue(key, out DataWrapper<TV> dataWrapper);
            if (!valExist) Debug.LogError($"DataWrapperMap: Key:{key} in {typeof(TV).Name} not found!");
            if (dataWrapper != null) return dataWrapper.Value;
            return default;
        }

        public void SetData(TK key, TV val)
        {
            if (_data.TryGetValue(key, out var dataWrapper))
            {
                dataWrapper.Value = val;
                Invoke();
            }
            else
            {
                _data[key] = new DataWrapper<TV>(val);
            }
        }

        public TV DeleteData(TK key)
        {
            if (_data.TryGetValue(key, out var dataWrapper))
            {
                _data.Remove(key);
                return dataWrapper.Value;
            }

            return default;
        }

        public void AddListener(TK key, Action<TV> func)
        {
            bool valExist = _data.TryGetValue(key, out var dataWrapper);
            if (!valExist) Debug.LogError($"DataWrapperMap: Key:{key} in {typeof(TV).Name} not found!");
            dataWrapper?.AddListener(func);
        }


        public void RemoveListener(TK key, Action<TV> func)
        {
            bool valExist = _data.TryGetValue(key, out var dataWrapper);
            if (!valExist) Debug.LogError($"DataWrapperMap: Key:{key} in {typeof(TV).Name} not found!");
            dataWrapper?.RemoveListener(func);
        }

        public void Invoke(TK key)
        {
            bool valExist = _data.TryGetValue(key, out var dataWrapper);
            if (!valExist) Debug.LogError($"DataWrapperMap: Key:{key} in {typeof(TV).Name} not found!");
            dataWrapper?.Invoke();
        }

        public new DataWrapperDictionary<TK, TV> DontReset()
        {
            _volatile = false;
            return this;
        }
        
        public override void Reset()
        {
            if (_volatile)
            {
                foreach (var item in _data)
                {
                    item.Value.Reset();
                }

                _data.Clear();
            }
        }
    }
}