using System;
using System.Collections.Generic;
using Runtime.PKGameCore.PKTools;
using UnityEngine;

namespace Runtime.PKGameCore.PKInput
{
    public class InputManager : SingletonBehaviour<InputManager>
    {
        private Dictionary<string, List<IInputReceiver>> _inputReceiverDic;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            _inputReceiverDic = new Dictionary<string, List<IInputReceiver>>();
        }

        protected override void UninitSingleton()
        {
            base.UninitSingleton();
            _inputReceiverDic.Clear();
        }

        public void Update()
        {
            if (PkGameTime.IsPause && !Inited) return;

            foreach (var receivers in _inputReceiverDic)
            {
                foreach (var receiver in receivers.Value)
                {
                    receiver.OnHorizontalAxisInput(Input.GetAxisRaw("Horizontal"));
                    receiver.OnVerticalAxisInput(Input.GetAxisRaw("Vertical"));
                    receiver.OnJumpInput(Input.GetKeyDown(KeyCode.Space));
                }
            }
        }

        public void Register(IInputReceiver inputReceiver)
        {
            if (_inputReceiverDic == null)
            {
                Debug.LogError("InputManager: InputDic is null!");
                return;
            }

            var receiverName = inputReceiver.GetType().Name;
            if (_inputReceiverDic.TryGetValue(receiverName, out var receiverList))
            {
                var index = receiverList.FindIndex(s => s.Id == inputReceiver.Id);
                if (index == -1)
                {
                    inputReceiver.OnStartControl();
                    receiverList.Add(inputReceiver);
                }
                else
                {
                    Debug.LogError($"InputManager: {receiverName} is Registered!");
                }
            }
            else
            {
                _inputReceiverDic[receiverName] = new List<IInputReceiver> { inputReceiver };
                inputReceiver.OnStartControl();
            }
        }

        public void Unregister(IInputReceiver inputReceiver)
        {
            var receiverName = inputReceiver.GetType().Name;
            if (_inputReceiverDic == null)
            {
                Debug.LogError("InputManager: InputDic is null!");
                return;
            }

            if (_inputReceiverDic.TryGetValue(receiverName, out var receiverList))
            {
                var index = receiverList.FindIndex(s => s.Id == inputReceiver.Id);
                if (index == -1)
                {
                    Debug.LogError($"InputManager: {receiverName} isn`t Register!");
                }
                else
                {
                    receiverList.RemoveAt(index);
                    inputReceiver.OnEndInputControl();
                }
            }
            else
            {
                Debug.LogError($"InputManager: {receiverName} isn`t Register!");
            }
        }
    }
}