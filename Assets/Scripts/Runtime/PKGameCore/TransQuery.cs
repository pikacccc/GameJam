using UnityEngine;

namespace Runtime.PKGameCore
{
    public static class TransQuery
    {
        public static Transform QueryTransform(Transform root, string key)
        {
            if (!root)
                return null;

            var result = root.Find(key);
            if (result != null) return result;
            for (int i = 0; i < root.childCount; i++)
            {
                result = QueryTransform(root.GetChild(i), key);
                if (result != null) break;
            }

            return result;
        }

        public static T Query<T>(Transform root, string key) where T : Component
        {
            Transform trans = QueryTransform(root, key);

            return trans != null ? trans.GetComponent<T>() : null;
        }
    }
}