using Runtime.PKGameCore.PKResourceManager;
using UnityEngine;

namespace Runtime.GamePlay.BuffSystem
{
    public enum BuffUpdateTimeEnum
    {
        Add,
        Replace,
        Keep
    }

    public enum BuffRemoveStackUpdateEnum
    {
        Clear,
        Reduce
    }

    public class BuffInfo
    {
        public BuffData buffData;
        public GameObject creator;
        public GameObject target;
        public float durationTimer;
        public float tickTimer;
        public int curStack = 1;
    }

    public class DamageInfo
    {
        public GameObject creator;
        public GameObject target;
        public float damage;
    }

    public static class BuffUtil
    {
        public static string BuffPath = "Buff/";

        public static BuffInfo AddBuff(string buffName, GameObject creator, GameObject target = null)
        {
            BuffData data = ResourceManager.Instance.LoadScriptableObj<BuffData>(BuffPath + buffName);
            var buff = new BuffInfo();
            buff.creator = creator;
            buff.target = target;
            buff.curStack = 1;
            buff.buffData = data;
            buff.tickTimer = data.tickTime;
            buff.durationTimer = data.duration;
            return buff;
        }
    }
}