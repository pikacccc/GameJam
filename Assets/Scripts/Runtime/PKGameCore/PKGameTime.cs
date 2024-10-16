using UnityEngine;

namespace Runtime.PKGameCore
{
    public static class PkGameTime
    {
        public static bool IsPause;
        public static float GameTime => IsPause ? 0f : Time.deltaTime;
    }
}