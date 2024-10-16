using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.PKGameCore.PKTools;
using UnityEngine;

namespace Runtime.PKGameCore.PKGameInit
{
    public class GameInit : SingletonBehaviour<GameInit>
    {
        public List<GameObject> managerPrefabs;

        private List<ISingletonBehaviour> _instanceList;

        private bool _isPrepareOk;

        private void InitData()
        {
            _instanceList = new List<ISingletonBehaviour>();
            _isPrepareOk = false;
        }

        protected override void InitSingleton()
        {
            base.InitSingleton();
            DontDestroyOnLoad(gameObject);
            InitData();
            InitManagers();
            _isPrepareOk = true;
        }

        protected override void UninitSingleton()
        {
            base.UninitSingleton();
            UnInitManagers();
            _isPrepareOk = false;
        }

        //初始化游戏管理的单例
        private void InitManagers()
        {
            if (_isPrepareOk) return;
            //初始化以prefab实例化出来的管理类
            foreach (var item in managerPrefabs)
            {
                var instance = Instantiate(item, transform);
                InitSingleton(instance);
            }
        }

        private void UnInitManagers()
        {
            for (int i = _instanceList.Count - 1; i >= 0; ++i)
            {
                var instance = _instanceList[i];
                try
                {
                    instance.Uninit();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                DestroyImmediate(instance.gameObject);
            }

            _instanceList = null;
        }

        //某些管理器下有子管理器，需要一并初始化
        private void InitSingleton(GameObject obj)
        {
            List<ISingletonBehaviour> childSingleton =
                obj.GetComponentsInChildren<ISingletonBehaviour>().ToList();
            foreach (var singleton in childSingleton)
            {
                singleton.Init();
                _instanceList.Add(singleton);
            }
        }
    }
}