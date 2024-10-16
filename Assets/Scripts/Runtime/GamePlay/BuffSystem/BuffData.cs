using UnityEngine;

namespace Runtime.GamePlay.BuffSystem
{
    [CreateAssetMenu(fileName = "BuffData", menuName = "BuffSystem/BuffData", order = 1)]
    public class BuffData : ScriptableObject
    {
        //基本信息
        public int id;
        public string buffName;
        public string desc;
        public Sprite icon;
        public int priority;
        public int maxStack;
        public string[] tags;

        //时间信息
        public bool isFovever;
        public float duration;
        public float tickTime;

        //更新方式
        public BuffUpdateTimeEnum buffUpdateTime;
        public BuffRemoveStackUpdateEnum buffRemoveStackUpdate;
        
        //基础回调点
        public BaseBuffModule OnCreate;
        public BaseBuffModule OnRemove;
        public BaseBuffModule OnTick;
        
        //伤害回调点
        public BaseBuffModule OnHit;
        public BaseBuffModule OnBehurt;
        public BaseBuffModule OnKill;
        public BaseBuffModule OnBekill;
    }
}