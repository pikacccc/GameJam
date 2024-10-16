using System.Collections.Generic;
using System.Linq;
using Runtime.PKGameCore.PKResourceManager;
using Runtime.PKGameCore.PKTools;
using Runtime.PKGameCore.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Runtime.PKGameCore.PKUI
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [SerializeField] public Camera uiCamera;
        [SerializeField] private Canvas canvasRoot;
        [SerializeField] private Transform norFormList;
        [SerializeField] private Transform overlayFormList;
        [SerializeField] private Transform formCache;

        private const string FormPath = "Forms/";

        protected override void InitSingleton()
        {
            base.InitSingleton();
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(uiCamera);
        }

        protected override void UninitSingleton()
        {
            base.UninitSingleton();
            DestroyAllWindows();
        }

        public T GetOrCreateForm<T>() where T : UIFormBase
        {
            return GetOrCreateForm(typeof(T).Name) as T;
        }

        public UIFormBase GetOrCreateForm(string formName)
        {
            UIFormBase form;
            if (TryGetFromCache(formName, out GameObject formObj))
            {
                form = formObj.GetComponent<UIFormBase>();
                AttachForm(form);
                Sort(form.formType);
                form.Init();
                form.Focus(true);
            }
            else
            {
                formObj = ResourceManager.Instance.InstantiatePrefab(FormPath + formName + "/" + formName);
                formObj.name = formName;
                form = formObj.GetComponent<UIFormBase>();
                AttachForm(form);
                Sort(form.formType);
                form.Setup();
                form.Init();
                form.Focus(true);
            }

            return form;
        }

        public bool TryRecycleForm<T>() where T : UIFormBase
        {
            return TryRecycleForm(typeof(T).Name);
        }

        public bool TryRecycleForm(string formName)
        {
            if (TryFindForm(formName, out UIFormBase form))
            {
                if (!form.Recycling)
                {
                    form.Recycling = true;
                    form.transform.SetParent(formCache);
                    form.Focus(false);
                    form.Uninit();
                    form.Recycling = false;
                    return true;
                }

                Debug.LogError($"UIManager: {formName} is Recycling!");
                return false;
            }

            Debug.LogError($"UIManager: {formName} Don`t Found!");
            return false;
        }

        private bool TryGetFromCache(string formName, out GameObject formObj)
        {
            Transform trans = formCache.Find(formName);
            if (trans == null)
            {
                formObj = null;
                return false;
            }

            formObj = trans.gameObject;
            return true;
        }

        private void AttachForm<T>(T form) where T : UIFormBase
        {
            Transform parent = null;
            switch (form.formType)
            {
                case FormType.Normal:
                    parent = norFormList;
                    break;
                case FormType.Overlay:
                    parent = overlayFormList;
                    break;
            }

            RectTransform formTrans = form.transform as RectTransform;
            if (formTrans != null)
            {
                formTrans.anchorMin = Vector2.zero;
                formTrans.anchorMax = Vector2.one;
                Util.SetParent(formTrans, parent);
                formTrans.sizeDelta = Vector2.zero;
            }
        }

        private void Sort(FormType formType)
        {
            Transform parent = null;
            string layerPrefix = "";
            switch (formType)
            {
                case FormType.Normal:
                    parent = norFormList;
                    layerPrefix = "NormalUI";
                    break;
                case FormType.Overlay:
                    parent = overlayFormList;
                    layerPrefix = "OverlayUI";
                    break;
            }

            if (parent != null)
                for (int i = 0; i < parent.childCount; ++i)
                {
                    List<Canvas> canvasList = parent.GetChild(i).GetComponentsInChildren<Canvas>().ToList();
                    foreach (var singleCanvas in canvasList)
                    {
                        singleCanvas.sortingLayerName = layerPrefix + $"_{(i + 1).ToString()}";
                    }
                }
        }

        private bool TryFindForm<T>(string formName, out T form) where T : UIFormBase
        {
            Transform formTrans = null;
            formTrans = norFormList.Find(formName);
            if (formTrans == null) formTrans = overlayFormList.Find(formName);

            if (formTrans == null)
            {
                form = default;
                return false;
            }

            form = formTrans.GetComponent<T>();
            return true;
        }

        public void DestroyAllWindows()
        {
            List<Transform> transformList = new List<Transform>();

            transformList.AddRange(formCache.Cast<Transform>());
            transformList.AddRange(norFormList.Cast<Transform>().Reverse());
            transformList.AddRange(overlayFormList.Cast<Transform>().Reverse());

            foreach (var winTransform in transformList)
            {
                var window = winTransform.GetComponent<UIFormBase>();
                if (window != null)
                {
                    DestroyWindow(window, true, true);
                }
                else
                {
                    DestroyImmediate(winTransform.gameObject);
                }
            }
        }

        private void DestroyWindow(UIFormBase form, bool immediate = false, bool changeLevel = false)
        {
            form.transform.SetParent(null);
            DestroyImmediate(form.gameObject);
        }

        public Canvas GetCanvasRoot()
        {
            return canvasRoot;
        }
    }
}