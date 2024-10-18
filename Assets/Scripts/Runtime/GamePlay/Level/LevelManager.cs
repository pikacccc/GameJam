using System;
using Runtime.PKGameCore;
using Runtime.PKGameCore.PKResourceManager;
using Runtime.PKGameCore.PKTools;
using UnityEngine;

namespace Runtime.GamePlay.Level
{
    public class LevelManager : SingletonBehaviour<LevelManager>
    {
        private const string LevelPath = "Levels/";

        public LevelData levelData;
        public int curLevelIndex;
        private int _maxLevelCount => levelData.levelList.Count;
        private bool _playing = false;

        [NonSerialized] public LevelBase CurLevel;

        private void Start()
        {
            Init();
        }

        protected override void InitSingleton()
        {
            base.InitSingleton();
            curLevelIndex = 0;
            InitLevel();
        }

        public void InitLevel()
        {
            if (CurLevel != null) Destroy(CurLevel.gameObject);
            var tempLevelPath = LevelPath + levelData.levelList[curLevelIndex];
            var levelObj = ResourceManager.Instance.InstantiatePrefab(tempLevelPath, null, transform.position,
                Quaternion.identity, transform);
            CurLevel = levelObj.GetComponent<LevelBase>();
            CurLevel.OnLevelInit();
        }

        public void StartLevel()
        {
            CurLevel.OnLevelStart();
            _playing = true;
        }

        public void RestartLevel()
        {
            _playing = true;
        }

        public void PauseLevel()
        {
            _playing = false;
        }

        public void EndLevel(bool win)
        {
            _playing = false;
            //TODO:当关卡结算时会出现几种情况
            //1.胜利未通关
            //2.失败
            //3.通关
            //这几种都应该由不同的表现
            if (win && curLevelIndex + 1 == _maxLevelCount)
            {
            }
            else if (win && curLevelIndex + 1 < _maxLevelCount)
            {
            }
            else if (!win)
            {
            }
        }

        public void JumpLevel(bool win)
        {
            if (win) curLevelIndex++;
            InitLevel();
            StartLevel();
        }

        private void Update()
        {
            if (!Inited) return;
            if (PkGameTime.IsPause && !_playing) return;
            CurLevel.Tick(PkGameTime.GameTime);
        }

        private void FixedUpdate()
        {
            if (!Inited) return;
            if (PkGameTime.IsPause && !_playing) return;
            CurLevel.FixedTick(PkGameTime.GameTime);
        }

        private void LateUpdate()
        {
            if (!Inited) return;
            if (PkGameTime.IsPause && !_playing) return;
            CurLevel.LateTick(PkGameTime.GameTime);
        }
    }
}