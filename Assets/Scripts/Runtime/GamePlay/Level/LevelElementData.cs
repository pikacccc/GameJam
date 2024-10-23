using System;
using System.Collections.Generic;
using Runtime.GamePlay.Ball;
using Runtime.GamePlay.BuffSystem;
using UnityEngine;

namespace Runtime.GamePlay.Level
{
    [Serializable]
    public class LevelElementData
    {
        public List<BuffData> oriBuffList;
        public GameObject oriBallPoint;
        public GameObject oriRacketPoint;
    }
}