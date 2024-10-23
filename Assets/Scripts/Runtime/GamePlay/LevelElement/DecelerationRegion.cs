using Runtime.GamePlay.Ball;
using Runtime.GamePlay.Level;
using UnityEngine;

namespace Runtime.GamePlay.LevelElement
{
    public class DecelerationRegion : LevelElementBase
    {
        private void OnTriggerStay2D(Collider2D other)
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