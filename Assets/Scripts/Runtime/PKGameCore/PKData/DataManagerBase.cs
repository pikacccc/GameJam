using System.Collections.Generic;
using Runtime.PKGameCore.PKTools;

namespace Runtime.PKGameCore.PKData
{
    public abstract class DataManagerBase : SingletonBehaviour<DataManagerBase>
    {
        protected override void InitSingleton()
        {
            base.InitSingleton();
            InitData();
        }

        protected override void UninitSingleton()
        {
            base.UninitSingleton();
            UninitData();
        }

        protected virtual void InitData()
        {
        }

        protected virtual void UninitData()
        {
        }
    }
}