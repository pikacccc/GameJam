using System;
using Runtime.GamePlay.Level;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.GamePlay.Ball
{
    public class Racket : LevelElementBase
    {
        public float speed;
        private bool _canMove = false;
        private Vector3 _dir = Vector3.zero;

        public override void OnLevelStart()
        {
            base.OnLevelStart();
            _canMove = true;
        }

        public void SetMoveStatus(bool isMove)
        {
            _canMove = isMove;
        }
        
        public override void Tick(float time)
        {
            base.Tick(time);
            if (!_canMove) return;
            _dir = Vector3.zero;
            var dirY = Vector3.zero;
            var dirX = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                dirY = Vector3.up;
            }

            if (Input.GetKey(KeyCode.S))
            {
                dirY = Vector3.down;
            }

            if (Input.GetKey(KeyCode.D))
            {
                dirX = Vector3.right;
            }

            if (Input.GetKey(KeyCode.A))
            {
                dirX = Vector3.left;
            }

            _dir = (dirX + dirY).normalized;
        }

        public override void FixedTick(float time)
        {
            base.FixedTick(time);
            if (!_canMove) return;
            Move(time);
        }

        private void Move(float time)
        {
            if (_dir.magnitude != 0)
            {
                transform.position += _dir * (speed * time);
            }
        }
    }
}