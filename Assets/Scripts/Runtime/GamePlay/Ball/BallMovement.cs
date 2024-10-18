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

        [NonSerialized]public Vector3 MoveDir;

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
            Move(time);
        }

        private void Move(float time)
        {
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

        public void CameraRebounds()
        {
            // var camera = CameraManager.Instance.
        }
    }
}