using System;
using UnityEngine.Serialization;

namespace Runtime.GamePlay.LevelElement
{
    [Serializable]
    public enum ElementType
    {
        Partner = 0,
        Neutral = 1,
        Enemy = 2
    }

    [Serializable]
    public class ElementData
    {
        public ElementType elementType;
        public float reboundsSpeedRate = 1f;
        public float visibleRange;
        public float visibleTime;
        public float visibleDecayRate;
        public float acceleratingRate;
        public float acceleratingMaxSpeed;
        public float decelerationRate;
        public float decelerationMinSpeed;
        public bool isPeriodicDisplay;
        public float period;
        public bool isDestructible = false;
    }
}