using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.PKGameCore.PKData
{
    public class DataWrapper<T> : DataWrapperBase<T>
    {
        public T Value
        {
            get => _data;
            set
            {
                _data = value;
                Invoke();
            }
        }

        public DataWrapper()
        {
            _data = default;
        }

        public DataWrapper(T data)
        {
            _data = data;
        }
        
        public new DataWrapper<T> DontReset()
        {
            _volatile = false;
            return this;
        }
    }
}