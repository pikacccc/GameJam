using System;

namespace Runtime.PKGameCore.PKData
{
    //数据包装类，主要用于数据驱动型的场景中
    public abstract class DataWrapperBase<T>
    {
        protected bool _volatile = true;
        protected T _data;
        protected event Action<T> Listener;

        public void AddListener(Action<T> func, bool immediateTrigger = false)
        {
            Listener += func;
            if (immediateTrigger) func?.Invoke(_data);
        }

        public void RemoveListener(Action<T> func)
        {
            Listener -= func;
        }

        public void Invoke()
        {
            Listener?.Invoke(_data);
        }

        public DataWrapperBase<T> DontReset()
        {
            _volatile = false;
            return this;
        }

        public virtual void Reset()
        {
            if (_volatile)
            {
                _data = default;
                Listener = null;
            }
        }
    }
}