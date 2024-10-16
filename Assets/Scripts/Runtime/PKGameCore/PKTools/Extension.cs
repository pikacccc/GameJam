using UnityEngine;

namespace Runtime.PKGameCore.PKTools
{
    public static class Extension
    {
        public static void SetAlpha(this SpriteRenderer render, float a)
        {
            var color = render.color;
            color = new Color(color.r, color.g, color.b, a);
            render.color = color;
        }
        
        public static Rect MergeRect(Rect r1, Rect r2)
        {
            Rect ret = r1;
            ret.x = Mathf.Min(r1.x, r2.x);
            ret.y = Mathf.Min(r1.y, r2.y);

            ret.xMax = Mathf.Max(r1.xMax, r2.xMax);
            ret.yMax = Mathf.Max(r1.yMax, r2.yMax);
            return ret;
        }
        
        public static void SetDefault(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }

        public static void SetWidth(this RectTransform rectTransform, float width)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = width;
            rectTransform.sizeDelta = sizeDelta;
        }
        
    }
}