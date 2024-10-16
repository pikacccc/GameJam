using UnityEngine;

namespace Runtime.PKGameCore.PKTools
{
    public abstract class ISingletonBehaviour : MonoBehaviour
    {
        public abstract void Init();
        public abstract void Uninit();

        protected virtual void InitSingleton()
        {
        }

        protected virtual void UninitSingleton()
        {
        }
    }

    public abstract class SingletonBehaviour<T> : ISingletonBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance => _instance;

        public bool Inited { get; protected set; } = false;

        protected virtual void Awake()
        {
            _instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }

        public override void Init()
        {
            InitSingleton();
            Inited = true;
        }

        public override void Uninit()
        {
            UninitSingleton();
            Inited = false;
            _instance = null;
        }
    }
}