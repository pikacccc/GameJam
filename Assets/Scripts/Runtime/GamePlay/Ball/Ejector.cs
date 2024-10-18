using System;
using System.Collections.Generic;
using Runtime.GamePlay.Level;
using UnityEngine;

namespace Runtime.GamePlay.Ball
{
    //发射器
    public class Ejector : LevelElementBase
    {
        [NonSerialized] public Vector3 shootDir;
        [NonSerialized] public List<BallMovement> ballList;

        private bool _isShoot;

        public override void OnLevelInit(LevelElementData elementData)
        {
            base.OnLevelInit(elementData);
            _isShoot = false;
            //test
            shootDir = new Vector3(1, -1, 0).normalized;
        }

        public override void Tick(float time)
        {
            base.Tick(time);
            if (Input.GetKeyDown(KeyCode.Space) && !_isShoot)
            {
                _isShoot = true;
                Shoot();
            }
        }

        public void Shoot()
        {
            foreach (var ball in ballList)
            {
                ball.MoveDir = shootDir;
                ball.SetMoveStatus(true);
            }
        }
    }
}