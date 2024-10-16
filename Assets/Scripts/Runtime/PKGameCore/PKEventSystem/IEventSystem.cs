using System;

namespace Runtime.PKGameCore.PKEventSystem
{
    public interface IEventSystem
    {
        void AddListener(string eventName, Action listener);

        void RemoveListener(string eventName, Action listener);

        void Broadcast(string eventName);

        void AddListener<T>(string eventName, Action<T> listener);

        void RemoveListener<T>(string eventName, Action<T> listener);

        void Broadcast<T>(string eventName, T param);

        /*
         * 多参数的情况，可以增加对应的AddListener/RemoveLinstener/Broadcast
         * 或者将这些多参数包装在一个类里面
        void AddListener<T1, T2>(string eventName, Action<T1, T2> listener);

        void RemoveListener<T1, T2>(string eventName, Action<T1, T2> listener);

        void Broadcast<T1, T2>(string eventName, T1 param1, T2 param2);
         */

        void ClearAll();
    }
}