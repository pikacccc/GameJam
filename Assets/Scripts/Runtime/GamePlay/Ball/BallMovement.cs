using System;
using Runtime.GamePlay.Level;
using Runtime.PKGameCore;
using Runtime.PKGameCore.PKCamera;
using UnityEngine;

namespace Runtime.GamePlay.Ball
{
    public class BallMovement : LevelElementBase
    {
        public BallAttr ballAttr;

        public LayerMask collLayer;

        [NonSerialized] public Vector3 MoveDir;

        private float _radius = 0.5f;

        private Vector3 _dis = Vector3.zero;

        private bool _inCam = true;
        private bool _camRebounds = false;

        public override void OnLevelInit(LevelElementData elementData)
        {
            ballAttr = elementData.ballAttr;
        }

        public override void OnLevelRestart()
        {
            SetMoveStatus(true);
        }

        public override void OnLevelPause()
        {
            SetMoveStatus(false);
        }

        public override void OnLevelEnd()
        {
            SetMoveStatus(false);
        }

        public void SetMoveStatus(bool isMove)
        {
            ballAttr.isMove = isMove;
        }

        public override void Tick(float time)
        {
            base.Tick(time);
            if (!ballAttr.isMove) return;
        }

        public override void FixedTick(float time)
        {
            base.FixedTick(time);
            if (!ballAttr.isMove) return;
            CheckBallInCam();
            CameraRebounds();
            Move(time);
        }

        private void Move(float time)
        {
            // var camera = CameraManager.Instance.MainCamera;
            // if (CameraManager.Instance.IsMove)
            // {
            //     if (_inCam)
            //     {
            //         _dis = transform.position - camera.transform.position;
            //         transform.position += MoveDir * (ballAttr.speed * time);
            //     }
            //     else
            //     {
            //         transform.position = camera.transform.position + _dis;
            //     }
            // }
            // else
            // {
            //     transform.position += MoveDir * (ballAttr.speed * time);
            // }
            transform.position += MoveDir * (ballAttr.speed * time);
        }

        private void Rebounds(Vector3 nor)
        {
            MoveDir = Vector3.Reflect(MoveDir, nor);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            int otherLayer = other.gameObject.layer;
            Vector3 nor = other.contacts[0].normal;
            if ((collLayer & (1 << otherLayer)) != 0 && ballAttr.curReboundsCount < ballAttr.maxReboundsCount)
            {
                Rebounds(nor);
                ballAttr.curReboundsCount++;
            }
        }

        public void CheckBallInCam()
        {
            var camera = CameraManager.Instance.MainCamera;
            var scale = transform.localScale.x;
            var tempIncam = true;
            for (int i = 0; i < 4; ++i)
            {
                var pos = transform.position;
                switch (i)
                {
                    case 0:
                        pos += Vector3.left * _radius * scale;
                        break;
                    case 1:
                        pos += Vector3.up * _radius * scale;
                        break;
                    case 2:
                        pos += Vector3.right * _radius * scale;
                        break;
                    case 3:
                        pos += Vector3.down * _radius * scale;
                        break;
                }

                Vector3 viewportPos = camera.WorldToViewportPoint(pos);
                if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
                {
                    tempIncam = false;
                    break;
                }
            }

            _inCam = tempIncam;

            if (_inCam) _camRebounds = false;
        }

        public void CameraRebounds()
        {
            if (_camRebounds) return;
            if(_inCam) return;
            _camRebounds = true;
            var camera = CameraManager.Instance.MainCamera;
            var scale = transform.localScale.x;
            for (int i = 0; i < 4; ++i)
            {
                var pos = transform.position;
                switch (i)
                {
                    case 0:
                        pos += Vector3.left * _radius * scale;
                        break;
                    case 1:
                        pos += Vector3.up * _radius * scale;
                        break;
                    case 2:
                        pos += Vector3.right * _radius * scale;
                        break;
                    case 3:
                        pos += Vector3.down * _radius * scale;
                        break;
                }

                Vector3 viewportPos = camera.WorldToViewportPoint(pos);
                Vector3 norX = Vector3.zero;
                Vector3 norY = Vector3.zero;
                if (viewportPos.x <= 0)
                {
                    norX = Vector3.right;
                }

                if (viewportPos.x >= 1)
                {
                    norX = Vector3.left;
                }

                if (viewportPos.y <= 0)
                {
                    norY = Vector3.up;
                }

                if (viewportPos.y >= 1)
                {
                    norY = Vector3.down;
                }

                Vector3 nor = (norX + norY).normalized;
                if (nor.magnitude != 0)
                {
                    Rebounds(nor);
                }
            }
        }
    }
}