using System;
using UnityEngine.Serialization;

namespace Runtime.GamePlay.Ball
{
    [Serializable]
    public class BallAttr
    {
        public bool isMove;
        public float speed;
        public int maxReboundsCount;
        public int curReboundsCount;
    }
}