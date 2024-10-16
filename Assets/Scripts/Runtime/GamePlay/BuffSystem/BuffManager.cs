using Runtime.PKGameCore;
using UnityEngine;

namespace Runtime.GamePlay.BuffSystem
{
    public class BuffManager : MonoBehaviour
    {
        private BuffHandler _handler;

        private void Awake()
        {
            if (_handler == null) InitBuff();
        }
        
        private void FixedUpdate()
        {
            _handler.UpdateBuff(PkGameTime.GameTime);
        }

        private void OnDestroy()
        {
            _handler = null;
        }

        public void AddBuff(BuffInfo info)
        {
            if (_handler == null) InitBuff();
            if (_handler != null) _handler.AddBuff(info);
        }

        public void RemoveBuff(BuffInfo info)
        {
            if (_handler == null) InitBuff();
            if (_handler != null) _handler.RemoveBuff(info);
        }

        public void InitBuff()
        {
            _handler = new BuffHandler();
        }
    }
}