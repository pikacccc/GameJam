using System;
using System.Collections;
using Runtime.PKGameCore;
using Runtime.PKGameCore.PKTools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.GamePlay.Level
{
    public class LevelManager : SingletonBehaviour<LevelManager>
    {
        public LevelData levelData;
        public int curLevelIndex;
        private int MaxLevelCount => levelData.levelList.Count;
        private bool _playing = false;

        [NonSerialized] public LevelBase CurLevel;

        private bool _sceneInited = false;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            StartGame(curLevelIndex);
        }

        private IEnumerator InitLevel()
        {
            SceneManager.LoadScene(levelData.levelList[curLevelIndex]);
            yield return null;
            CurLevel = GameObject.Find(levelData.levelList[curLevelIndex]).GetComponent<LevelBase>();
            CurLevel.OnLevelInit();
            CurLevel.OnLevelStart();
            _sceneInited = true;
            _playing = true;
        }

        public void StartGame(int levelIndex = 0)
        {
            this.curLevelIndex = levelIndex;
            StartCoroutine(InitLevel());
        }

        public void RecoverLevel()
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
            if (win && curLevelIndex + 1 >= MaxLevelCount)
            {
            }
            else if (win && curLevelIndex + 1 < MaxLevelCount)
            {
                Jump(true);
            }
            else if (!win)
            {
                Jump(false);
            }
        }

        public void Jump(bool win)
        {
            if (win) curLevelIndex++;
            StartCoroutine(InitLevel());
        }

        private void Update()
        {
            if (!_sceneInited) return;
            if (PkGameTime.IsPause || !_playing) return;
            CurLevel.Tick(PkGameTime.GameTime);
        }

        private void FixedUpdate()
        {
            if (!_sceneInited) return;
            if (PkGameTime.IsPause || !_playing) return;
            CurLevel.FixedTick(PkGameTime.GameTime);
        }

        private void LateUpdate()
        {
            if (!_sceneInited) return;
            if (PkGameTime.IsPause || !_playing) return;
            CurLevel.LateTick(PkGameTime.GameTime);
        }
    }
}