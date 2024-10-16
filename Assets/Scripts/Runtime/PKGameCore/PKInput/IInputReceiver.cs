using System;
using UnityEngine;

namespace Runtime.PKGameCore.PKInput
{
    //抽象玩家的输入行为，由InputManager统一管理
    public interface IInputReceiver
    {
        public Guid Id { get; }
        public void OnHorizontalAxisInput(float horInput);
        public void OnVerticalAxisInput(float verInput);
        public void OnJumpInput(bool isJump);

        public void OnStartControl();
        public void OnEndInputControl();
    }
}