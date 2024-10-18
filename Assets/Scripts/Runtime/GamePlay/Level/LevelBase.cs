using System.Collections.Generic;
using System.Linq;
using Runtime.GamePlay.Ball;
using Runtime.PKGameCore.PKResourceManager;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.GamePlay.Level
{
    public class LevelBase : MonoBehaviour, ILevel
    {
        private const string ElementPath = "LevelElements/";

        public LevelElementData levelElementData;

        //静态的场景元素
        private List<LevelElementBase> _elements = new();

        //需要动态加载出来的场景元素
        private List<BallMovement> _balls = new();
        private LevelElementBase _racket;
        private Ejector _ejector;

        public void OnLevelInit()
        {
            _elements = GetComponentsInChildren<LevelElementBase>().ToList();

            InitBall();
            InitRacket();
            InitEjector();

            foreach (var element in _elements)
            {
                element.OnLevelInit(levelElementData);
            }

            foreach (var ball in _balls)
            {
                ball.OnLevelInit(levelElementData);
            }

            // _racket.OnLevelInit(levelElementData);
        }

        private void InitEjector()
        {
            var ejectorObj = ResourceManager.Instance.InstantiatePrefab(ElementPath + "Ejector", null,
                levelElementData.oriBallPoint.transform.position, Quaternion.identity, transform);
            _ejector = ejectorObj.GetComponent<Ejector>();
            _ejector.OnLevelInit(levelElementData);
            var tempBallList = new List<BallMovement>();
            foreach (var ball in _balls)
            {
                tempBallList.Add(ball);
            }

            _ejector.ballList = tempBallList;
        }

        private void InitBall()
        {
            GameObject ballObj = ResourceManager.Instance.InstantiatePrefab(ElementPath + "Ball", null,
                levelElementData.oriBallPoint.transform.position, Quaternion.identity, transform);
            _balls.Add(ballObj.GetComponent<BallMovement>());
        }

        private void InitRacket()
        {
        }

        public void OnLevelStart()
        {
            foreach (var element in _elements)
            {
                element.OnLevelStart();
            }

            foreach (var ball in _balls)
            {
                ball.OnLevelStart();
            }

            _racket.OnLevelStart();
        }

        public void OnLevelRestart()
        {
            foreach (var element in _elements)
            {
                element.OnLevelRestart();
            }

            foreach (var ball in _balls)
            {
                ball.OnLevelRestart();
            }

            _racket.OnLevelRestart();
        }

        public void OnLevelPause()
        {
            foreach (var element in _elements)
            {
                element.OnLevelPause();
            }

            foreach (var ball in _balls)
            {
                ball.OnLevelPause();
            }

            _racket.OnLevelPause();
        }

        public void OnLevelEnd()
        {
            foreach (var element in _elements)
            {
                element.OnLevelEnd();
            }

            foreach (var ball in _balls)
            {
                ball.OnLevelEnd();
            }

            _racket.OnLevelEnd();
        }

        public void Tick(float time)
        {
            foreach (var element in _elements)
            {
                element.Tick(time);
            }

            foreach (var ball in _balls)
            {
                ball.Tick(time);
            }

            _ejector.Tick(time);
            // _racket.Tick(time);
        }

        public void FixedTick(float time)
        {
            foreach (var element in _elements)
            {
                element.FixedTick(time);
            }

            foreach (var ball in _balls)
            {
                ball.FixedTick(time);
            }

            // _racket.FixedTick(time);
        }

        public void LateTick(float time)
        {
            foreach (var element in _elements)
            {
                element.LateTick(time);
            }

            foreach (var ball in _balls)
            {
                ball.LateTick(time);
            }

            // _racket.LateTick(time);
        }

        public LevelElementBase FindLevelElement(string elementName)
        {
            var tempElement = _elements?.Find(s => s.gameObject.name == elementName);
            if (tempElement == null)
            {
                tempElement = _balls?.Find(s => s.gameObject.name == elementName);
            }

            if (tempElement == null && tempElement.gameObject.name == _racket.gameObject.name)
            {
                tempElement = _racket;
            }

            return tempElement;
        }
    }
}