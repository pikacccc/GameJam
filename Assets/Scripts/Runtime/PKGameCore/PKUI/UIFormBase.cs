using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.PKGameCore.UI
{
    public enum FormType
    {
        Normal = 0,
        Overlay = 1
    }

    [RequireComponent(typeof(Canvas))]
    public abstract class UIFormBase : UIBehaviour
    {
        public FormType formType = FormType.Normal;

        private bool _init = false;

        public bool Recycling { get; set; } = false;

        public void Init()
        {
            if (!_init)
            {
                _init = true;
                OnInit();
            }
        }

        public void Uninit()
        {
            if (_init)
            {
                _init = false;
                OnUninit();
            }
        }

        public virtual void Setup()
        {
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnUninit()
        {
        }


        public virtual void Focus(bool on)
        {
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var component in GetComponentsInChildren<UnityEngine.UI.Button>(true)) component.onClick = null;
            foreach (var component in GetComponentsInChildren<UnityEngine.UI.Toggle>(true))
                component.onValueChanged = null;
        }
    }
}