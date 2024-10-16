using System.Collections.Generic;

namespace Runtime.GamePlay.BuffSystem
{
    public class BuffHandler
    {
        public LinkedList<BuffInfo> buffList = new LinkedList<BuffInfo>();

        public void UpdateBuff(float dt)
        {
            BuffTick(dt);
        }

        public void BuffTick(float dt)
        {
            List<BuffInfo> deleteBuffList = new List<BuffInfo>();
            foreach (var buffInfo in buffList)
            {
                if (buffInfo.buffData.OnTick != null)
                {
                    if (buffInfo.tickTimer < 0)
                    {
                        buffInfo.buffData.OnTick.Apply(buffInfo);
                        buffInfo.tickTimer = buffInfo.buffData.tickTime;
                    }
                    else
                    {
                        buffInfo.tickTimer -= dt;
                    }
                }

                if (buffInfo.buffData.OnRemove != null && !buffInfo.buffData.isFovever)
                {
                    if (buffInfo.durationTimer < 0)
                    {
                        deleteBuffList.Add(buffInfo);
                    }
                    else
                    {
                        buffInfo.durationTimer -= dt;
                    }
                }
            }

            foreach (var item in deleteBuffList)
            {
                RemoveBuff(item);
            }
        }

        public void AddBuff(BuffInfo buffInfo)
        {
            BuffInfo findBuffInfo = FindBuff(buffInfo.buffData.id);
            if (findBuffInfo != null)
            {
                if (findBuffInfo.curStack < findBuffInfo.buffData.maxStack)
                {
                    findBuffInfo.curStack++;
                    switch (findBuffInfo.buffData.buffUpdateTime)
                    {
                        case BuffUpdateTimeEnum.Add:
                            findBuffInfo.durationTimer += findBuffInfo.buffData.duration;
                            break;
                        case BuffUpdateTimeEnum.Replace:
                            findBuffInfo.durationTimer = findBuffInfo.buffData.duration;
                            break;
                        default:
                            break;
                    }

                    findBuffInfo.buffData.OnCreate?.Apply(findBuffInfo);
                }
            }
            else
            {
                // buffInfo.durationTimer = buffInfo.buffData.duration;
                buffInfo.buffData.OnCreate?.Apply(buffInfo);
                buffList.AddLast(buffInfo);
                BuffListSort(buffList);
            }
        }

        public void RemoveBuff(BuffInfo buffInfo)
        {
            BuffInfo findBuffInfo = FindBuff(buffInfo.buffData.id);
            if (findBuffInfo == null) return;
            switch (buffInfo.buffData.buffRemoveStackUpdate)
            {
                case BuffRemoveStackUpdateEnum.Clear:
                    findBuffInfo.buffData.OnRemove?.Apply(findBuffInfo);
                    buffList.Remove(findBuffInfo);
                    break;
                case BuffRemoveStackUpdateEnum.Reduce:
                    findBuffInfo.curStack--;
                    findBuffInfo.buffData.OnRemove?.Apply(findBuffInfo);
                    if (findBuffInfo.curStack == 0)
                    {
                        buffList.Remove(findBuffInfo);
                    }
                    else
                    {
                        buffInfo.durationTimer = buffInfo.buffData.duration;
                    }

                    break;
                default:
                    break;
            }
        }

        private BuffInfo FindBuff(int buffDataId)
        {
            foreach (var buff in buffList)
            {
                if (buff.buffData.id == buffDataId)
                {
                    return buff;
                }
            }

            return default;
        }

        //使用插入排序
        private void BuffListSort(LinkedList<BuffInfo> list)
        {
            if (list == null || list.Count < 2)
            {
                return;
            }

            LinkedListNode<BuffInfo> cur = list.First.Next;

            while (cur != null)
            {
                LinkedListNode<BuffInfo> next = cur.Next;
                LinkedListNode<BuffInfo> prev = cur.Previous;

                while (prev != null && prev.Value.buffData.priority > cur.Value.buffData.priority)
                {
                    prev = prev.Previous;
                }

                if (prev == null)
                {
                    list.Remove(cur);
                    list.AddFirst(cur);
                }
                else
                {
                    list.Remove(cur);
                    list.AddAfter(prev, cur);
                }

                cur = next;
            }
        }
    }
}