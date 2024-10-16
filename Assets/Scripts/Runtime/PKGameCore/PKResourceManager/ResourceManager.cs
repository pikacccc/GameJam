using Runtime.PKGameCore.PKTools;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.PKGameCore.PKResourceManager
{
    public class ResourceManager : SingletonBehaviour<ResourceManager>
    {
        private const string PrefabPath = "Prefabs/";
        private const string ScriptableObjPath = "ScriptableObjs/";

        protected override void UninitSingleton()
        {
            base.UninitSingleton();
            ClearAsset();
        }

        public void ClearAsset()
        {
            Resources.UnloadUnusedAssets();
        }

        public T LoadAsset<T>(string path) where T : Object
        {
            var asset = Resources.Load<T>(path);
            if (asset == null)
            {
                Debug.LogError($"Resource not find! path:{path}");
            }

            return asset;
        }

        public T LoadScriptableObj<T>(string refPath) where T : Object
        {
            T obj = LoadAsset<T>(ScriptableObjPath + refPath);
            return obj;
        }

        public GameObject InstantiatePrefab(string refPath, System.Action<GameObject> callback = null,
            Vector3? position = default, Quaternion? rotation = default, Transform parent = null)
        {
            GameObject prefab = LoadAsset<GameObject>(PrefabPath + refPath);
            return prefab ? DoInstantiate(prefab, callback, position, rotation, parent) : null;
        }

        private GameObject DoInstantiate(GameObject prefab, System.Action<GameObject> callback = null,
            Vector3? position = default, Quaternion? rotation = default, Transform parent = null)
        {
            GameObject result = null;
            if (prefab != null)
            {
                result = Instantiate(prefab);
            }

            if (result != null)
            {
                result.name = prefab.name;
                if (parent != null) result.transform.SetParent(parent, false);
                if (position != null) result.transform.localPosition = position.Value;
                if (rotation != null) result.transform.localRotation = rotation.Value;
            }

            callback?.Invoke(result);
            return result;
        }
    }
}