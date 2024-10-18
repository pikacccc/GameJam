using UnityEngine;

namespace Runtime.GamePlay.Level
{
    public interface ILevel
    {
        public void OnLevelInit();
        public void OnLevelStart();
        public void OnLevelRestart();
        public void OnLevelPause();
        public void OnLevelEnd();
        public void Tick(float time);
        public void FixedTick(float time);
        public void LateTick(float time);
        public LevelElementBase FindLevelElement(string elementName);
    }
}