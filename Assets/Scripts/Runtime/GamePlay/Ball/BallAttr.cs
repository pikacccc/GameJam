using System;
using UnityEditor.Rendering;
using UnityEngine.Serialization;

namespace Runtime.GamePlay.Ball
{
    [Serializable]
    public class BallAttr
    {
        public bool isMove;
        public float speed;
        public float maxSpeed;
        public float minSpeed;
        public int maxReboundsCount;
        public int curReboundsCount;

        public int maxHp;
        public int curHp;
        [NonSerialized] public bool IsDead = false;
    }
}