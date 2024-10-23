using Runtime.GamePlay.LevelElement;
using UnityEngine;

namespace Runtime.GamePlay.Level
{
    public class LevelElementBase : MonoBehaviour
    {
        public ElementData data;
        private ObjectTimer _objectTimer;
        
        public virtual void OnLevelInit(LevelElementData elementData)
        {
            if (data.isPeriodicDisplay) _objectTimer = new ObjectTimer(this.gameObject, data.period);
        }

        public virtual void OnLevelStart()
        {
            if (data.isPeriodicDisplay) _objectTimer.StartTimer();
        }

        public virtual void OnLevelRestart()
        {
            if (data.isPeriodicDisplay) _objectTimer.StartTimer();
        }
        
        public virtual void OnLevelPause()
        {
            if (data.isPeriodicDisplay) _objectTimer.StopTimer();
        }

        public virtual void OnLevelEnd()
        {
            if (data.isPeriodicDisplay) _objectTimer.StopTimer();
        }

        public virtual void Tick(float time)
        {
        }

        public virtual void FixedTick(float time)
        {
            if (data.isPeriodicDisplay) _objectTimer.UpdateTimer(time);
        }

        public virtual void LateTick(float time)
        {
        }
    }
}