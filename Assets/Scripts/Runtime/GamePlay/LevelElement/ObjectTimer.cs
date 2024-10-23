using UnityEngine;

namespace Runtime.GamePlay.LevelElement
{
    public class ObjectTimer
    {
        private GameObject _targetObject;
        private float _interval;
        private float _timer;
        private bool _isTimerRunning;

        public ObjectTimer(GameObject obj, float interval)
        {
            _targetObject = obj;
            _interval = interval;
            _timer = 0f;
            _isTimerRunning = false;
        }

        public void StartTimer()
        {
            _isTimerRunning = true;
        }

        public void StopTimer()
        {
            _isTimerRunning = false;
        }

        public void ResetTimer()
        {
            _timer = 0f;
        }

        public void UpdateTimer(float time)
        {
            if (_isTimerRunning)
            {
                _timer += time;
                if (_timer >= _interval)
                {
                    _timer = 0f;
                    ToggleObjectVisibility();
                }
            }
        }

        private void ToggleObjectVisibility()
        {
            _targetObject.SetActive(!_targetObject.activeSelf);
        }
    }
}