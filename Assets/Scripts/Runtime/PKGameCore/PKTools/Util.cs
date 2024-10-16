using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Runtime.PKGameCore.PKTools
{
    public static class Util
    {
        public static int GenerateRandomByRange(Vector2 range)
        {
            int random = Mathf.FloorToInt(Random.Range(range.x, range.y));
            return random;
        }

        public static Vector2 ClampVector2(Vector2 min, Vector2 max, Vector2 vec)
        {
            return new Vector2(Mathf.Clamp(vec.x, min.x, max.x), Mathf.Clamp(vec.y, min.y, max.y));
        }

        public static void SetParent(Transform trans, Transform parent)
        {
            trans.SetParent(parent);
            trans.SetDefault();
        }

        //Action<Transform,int> callBack = (child,index) => void
        public static void ReserveChildren(Transform trans, int count, Action<Transform, int> callBack)
        {
            if (trans == null || trans.childCount <= 0 || count <= trans.childCount) return;
            for (var i = trans.childCount; i < count; ++i)
            {
                Object.Instantiate(trans.GetChild(0), trans);
            }

            for (int i = 0; i < trans.childCount; ++i)
            {
                trans.GetChild(i).gameObject.SetActive(i < count);
            }

            if (callBack == null) return;
            for (var i = 0; i < count; ++i)
            {
                callBack.Invoke(trans.GetChild(i), i);
            }
        }

        //Action<Transform,int> callBack = (child,index) => void
        public static void TraverseChildren(Transform trans, Action<Transform, int> callBack)
        {
            if (trans == null && callBack != null) return;
            for (var i = 0; i < trans.childCount; ++i)
            {
                callBack.Invoke(trans.GetChild(i), i);
            }
        }

        // public static void texture2DToMat(Texture2D texture, Mat mat)
        // {
        //     Color32[] colors = texture.GetPixels32();
        //     byte[] data = new byte[colors.Length * 3];
        //
        //     for (int i = 0; i < colors.Length; i++)
        //     {
        //         data[i * 3] = colors[i].r;
        //         data[i * 3 + 1] = colors[i].g;
        //         data[i * 3 + 2] = colors[i].b;
        //     }
        //
        //     mat.put(0, 0, data);
        // }
    }
}