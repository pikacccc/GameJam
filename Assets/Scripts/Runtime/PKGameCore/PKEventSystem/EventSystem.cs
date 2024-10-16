using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.PKGameCore.PKEventSystem
{
    public class EventSystem : IEventSystem
    {
        private readonly Dictionary<string, Delegate> eventMap = new();

        public void AddListener(string eventName, Action listener)
        {
            if (string.IsNullOrEmpty(eventName) || listener == null) return;

            if (!eventMap.TryGetValue(eventName, out var callbacks))
            {
                eventMap.Add(eventName, listener);
            }
            else
            {
                eventMap[eventName] = Delegate.Combine(callbacks, listener);
            }
        }

        public void RemoveListener(string eventName, Action listener)
        {
            if (string.IsNullOrEmpty(eventName) || listener == null) return;

            if (!eventMap.TryGetValue(eventName, out var callbacks)) return;

            eventMap[eventName] = Delegate.Remove(callbacks, listener);
        }

        public void Broadcast(string eventName)
        {
            if (string.IsNullOrEmpty(eventName)) return;

            if (!eventMap.TryGetValue(eventName, out var callbacks)) return;

            if (callbacks is not Action actions) return;

            try
            {
                actions.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void AddListener<T>(string eventName, Action<T> listener)
        {
            if (string.IsNullOrEmpty(eventName) || listener == null) return;

            if (!eventMap.TryGetValue(eventName, out var callbacks))
            {
                eventMap.Add(eventName, listener);
            }
            else
            {
                eventMap[eventName] = Delegate.Combine(callbacks, listener);
            }
        }

        public void RemoveListener<T>(string eventName, Action<T> listener)
        {
            if (string.IsNullOrEmpty(eventName) || listener == null) return;

            if (!eventMap.TryGetValue(eventName, out var callbacks)) return;

            eventMap[eventName] = Delegate.Remove(callbacks, listener);
        }

        public void Broadcast<T>(string eventName, T param)
        {
            if (string.IsNullOrEmpty(eventName)) return;

            if (!eventMap.TryGetValue(eventName, out var callbacks)) return;

            if (callbacks is not Action<T> actions) return;

            try
            {
                actions.Invoke(param);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void ClearAll()
        {
            eventMap.Clear();
        }
    }
}