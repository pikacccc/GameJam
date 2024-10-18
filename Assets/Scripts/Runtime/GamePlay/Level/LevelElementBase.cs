using System;
using UnityEngine;

namespace Runtime.GamePlay.Level
{
    public class LevelElementBase : MonoBehaviour
    {
        public virtual void OnLevelInit(LevelElementData elementData)
        {
        }

        public virtual void OnLevelStart()
        {
        }

        public virtual void OnLevelRestart()
        {
        }
        
        public virtual void OnLevelPause()
        {
        }

        public virtual void OnLevelEnd()
        {
        }

        public virtual void Tick(float time)
        {
        }

        public virtual void FixedTick(float time)
        {
        }

        public virtual void LateTick(float time)
        {
        }
    }
}