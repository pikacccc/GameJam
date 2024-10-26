using System;
using Runtime.GamePlay.Level;
using Runtime.GamePlay.LevelElement;
using Runtime.PKGameCore;
using Runtime.PKGameCore.PKCamera;
using UnityEngine;

namespace Runtime.GamePlay.Ball
{
    public class BallMovement : LevelElementBase
    {
        public GameObject damageVFX;
        public BallAttr ballAttr;

        public LayerMask collLayer;
        public LayerMask camWall;

        [NonSerialized] public Vector3 MoveDir;

        public override void OnLevelInit(LevelElementData elementData)
        {
            ballAttr.curHp = ballAttr.maxHp;
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
            if ((camWall & (1 << otherLayer)) != 0)
            {
                Rebounds(nor);
                return;
            }

            if ((collLayer & (1 << otherLayer)) != 0 && ballAttr.curReboundsCount < ballAttr.maxReboundsCount)
            {
                var element = other.gameObject.GetComponent<LevelElementBase>();
                ChangeSpeed(element.data.reboundsSpeedRate);
                Rebounds(nor);
                ballAttr.curReboundsCount++;
            }

            var levelElement = other.gameObject.GetComponent<LevelElementBase>();
            if (levelElement != null && levelElement.data.isDestructible)
            {
                other.gameObject.SetActive(false);
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            var levelElement = other.GetComponent<LevelElementBase>();
            if (levelElement == null) return;
            if (levelElement.data.elementType == ElementType.Enemy)
            {
                ballAttr.curHp--;
                if (ballAttr.curHp <= 0)
                {
                    LevelManager.Instance.EndLevel(false);
                }
                else
                {
                    Instantiate(damageVFX, transform.position, Quaternion.identity);
                    LevelManager.Instance.CurLevel.RecycleBall();
                }
            }

            other.gameObject.GetComponent<LevelElementBase>();
            if (levelElement.data.isDestructible)
            {
                other.gameObject.SetActive(false);
            }
        }

        private void ChangeSpeed(float rate)
        {
            var ballSpeed = ballAttr.speed * rate;
            var finaSpeed = ballSpeed;
            if (ballSpeed < ballAttr.minSpeed) finaSpeed = ballAttr.minSpeed;
            if (ballSpeed > ballAttr.maxSpeed) finaSpeed = ballAttr.maxSpeed;
            ballAttr.speed = finaSpeed;
        }
    }
}