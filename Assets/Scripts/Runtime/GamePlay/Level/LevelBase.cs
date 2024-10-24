using System.Collections.Generic;
using System.Linq;
using Runtime.GamePlay.Ball;
using Runtime.PKGameCore.PKCamera;
using Runtime.PKGameCore.PKResourceManager;
using UnityEngine;

namespace Runtime.GamePlay.Level
{
    public class LevelBase : MonoBehaviour, ILevel
    {
        private const string ElementPath = "LevelElements/";

        public LevelElementData levelElementData;

        //静态的场景元素
        private List<LevelElementBase> _elements = new();

        //需要动态加载出来的场景元素
        private BallMovement _ball;
        private Racket _racket;
        private Ejector _ejector;

        private bool _isShoot;

        public void OnLevelInit()
        {
            InitRacket();
            InitBall();
            InitEjector();

            _elements = GetComponentsInChildren<LevelElementBase>().ToList();
            foreach (var element in _elements)
            {
                element.OnLevelInit(levelElementData);
            }

            _ball.OnLevelInit(levelElementData);
            _racket.OnLevelInit(levelElementData);
            _isShoot = false;
        }

        private void InitEjector()
        {
            var ejectorObj = ResourceManager.Instance.InstantiatePrefab(ElementPath + "Ejector", null,
                _racket.transform.position + 0.5f * Vector3.down, Quaternion.identity, transform);
            ejectorObj.transform.SetParent(_racket.transform);
            _ejector = ejectorObj.GetComponent<Ejector>();
            _ejector.OnLevelInit(levelElementData);
            _ejector.Ball = _ball;
        }

        private void InitBall()
        {
            GameObject ballObj = ResourceManager.Instance.InstantiatePrefab(ElementPath + "Ball", null,
                _racket.transform.position + 0.5f * Vector3.down, Quaternion.identity, transform);
            _ball = ballObj.GetComponent<BallMovement>();
        }

        private void InitRacket()
        {
            GameObject racketObj = ResourceManager.Instance.InstantiatePrefab(ElementPath + "Racket", null,
                levelElementData.oriRacketPoint.transform.position, Quaternion.identity, transform);
            _racket = racketObj.GetComponent<Racket>();
        }

        public void OnLevelStart()
        {
            foreach (var element in _elements)
            {
                element.OnLevelStart();
            }

            _ball.OnLevelStart();
            _racket.OnLevelStart();
            CameraManager.Instance.SetMoveStatus(false);
            _ball.SetMoveStatus(false);
            _racket.SetMoveStatus(false);
        }

        public void OnLevelRestart()
        {
            foreach (var element in _elements)
            {
                element.OnLevelRestart();
            }


            _ball.OnLevelRestart();
            _racket.OnLevelRestart();
        }

        public void OnLevelPause()
        {
            foreach (var element in _elements)
            {
                element.OnLevelPause();
            }


            _ball.OnLevelPause();
            _racket.OnLevelPause();
        }

        public void OnLevelEnd()
        {
            foreach (var element in _elements)
            {
                element.OnLevelEnd();
            }

            _ball.OnLevelEnd();
            _racket.OnLevelEnd();
        }

        public void Tick(float time)
        {
            foreach (var element in _elements)
            {
                element.Tick(time);
            }

            _ball.Tick(time);
            _ejector.Tick(time);
            _racket.Tick(time);

            HandleInput();
        }

        public void FixedTick(float time)
        {
            foreach (var element in _elements)
            {
                element.FixedTick(time);
            }

            _ball.FixedTick(time);
            _racket.FixedTick(time);
            _ejector.FixedTick(time);
        }

        public void LateTick(float time)
        {
            foreach (var element in _elements)
            {
                element.LateTick(time);
            }

            _ball.LateTick(time);
            _racket.LateTick(time);
            _ejector.LateTick(time);
        }

        public LevelElementBase FindLevelElement(string elementName)
        {
            var tempElement = _elements?.Find(s => s.gameObject.name == elementName);
            if (tempElement == null && tempElement.gameObject.name == _ball.gameObject.name)
            {
                tempElement = _ball;
            }

            if (tempElement == null && tempElement.gameObject.name == _racket.gameObject.name)
            {
                tempElement = _racket;
            }

            return tempElement;
        }

        public void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!_isShoot)
                {
                    _ejector.IsShoot = true;
                    _ejector.gameObject.SetActive(false);
                    _ball.MoveDir = -_ejector.ray.up;

                    _racket.SetMoveStatus(true);
                    _ball.SetMoveStatus(true);
                    CameraManager.Instance.SetMoveStatus(true);

                    _isShoot = true;
                }
                else
                {
                    _ball.transform.position = (_racket.transform.position + 0.5f * Vector3.down);
                    _ejector.gameObject.SetActive(true);
                    _ejector.IsShoot = false;
                    if (_ball.ballAttr.IsDead)
                        _ball.gameObject.SetActive(true);
                    
                    _racket.SetMoveStatus(false);
                    _ball.SetMoveStatus(false);
                    CameraManager.Instance.SetMoveStatus(false);
                    
                    _isShoot = false;
                }
            }
        }
    }
}