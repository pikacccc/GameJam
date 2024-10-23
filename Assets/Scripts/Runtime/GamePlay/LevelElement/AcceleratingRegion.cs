using Runtime.GamePlay.Ball;
using Runtime.GamePlay.Level;
using UnityEngine;

namespace Runtime.GamePlay.LevelElement
{
    public class AcceleratingRegion : LevelElementBase
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                var ball = other.GetComponent<BallMovement>();
                if (ball.ballAttr.speed < data.acceleratingMaxSpeed)
                {
                    ball.ballAttr.speed += data.acceleratingRate;
                }
            }
        }
    }
}