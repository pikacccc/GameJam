﻿using System;
using System.Collections.Generic;
using Runtime.GamePlay.Level;
using Runtime.PKGameCore;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.GamePlay.Ball
{
    //发射器
    public class Ejector : LevelElementBase
    {
        [NonSerialized] public BallMovement Ball;

        [NonSerialized]public bool IsShoot;

        public Transform ray;
        public float rotateSpeed;
        public float minAngle = -75f; // 最小角度
        public float maxAngle = 75f; // 最大角度
        private bool _isForward = true; // 是否向前旋转

        public override void OnLevelInit(LevelElementData elementData)
        {
            base.OnLevelInit(elementData);
            IsShoot = false;
        }

        public override void FixedTick(float time)
        {
            base.FixedTick(time);
            if (IsShoot) return;
            if (_isForward)
            {
                ray.Rotate(0, 0, rotateSpeed * time); // 向前旋转
                if (ray.eulerAngles.z >= maxAngle && ray.eulerAngles.z <= 180)
                {
                    _isForward = false; // 到达最大角度，开始向后旋转
                }
            }
            else
            {
                ray.Rotate(0, 0, -rotateSpeed * time); // 向后旋转
                if (ray.eulerAngles.z <= 360 + minAngle && ray.eulerAngles.z >= 180)
                {
                    _isForward = true; // 到达最小角度，开始向前旋转
                }
            }
        }
    }
}