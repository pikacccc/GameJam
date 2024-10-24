using System;
using Cinemachine;
using Runtime.PKGameCore.PKTools;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Runtime.PKGameCore.PKCamera
{
    public class CameraManager : SingletonBehaviour<CameraManager>
    {
        [NonSerialized] public Camera MainCamera;

        public GameObject camBrainObj;
        public GameObject virCamObj;

        private CinemachineBrain _cameraBrain;
        private CinemachineVirtualCameraBase _mainCamera;

        private bool _isReady;

        private CinemachineVirtualCameraBase _curCam;

        public bool useZoom = true;
        [Range(0, 10)] public float zoomMultiplier = 1f;
        [Range(0, 100)] public float minZoom = 1f;
        [Range(0, 100)] public float maxZoom = 40f;
        public float cameraSize = 10f;

        public bool useMove = true;
        public float cameraSpeed;
        private Vector3 _dir = Vector3.zero;
        [NonSerialized] public bool IsMove = false;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            _cameraBrain = camBrainObj.GetComponent<CinemachineBrain>();
            _mainCamera = virCamObj.GetComponent<CinemachineVirtualCameraBase>();
            MainCamera = camBrainObj.GetComponent<Camera>();
            ((CinemachineVirtualCamera)_mainCamera).m_Lens.OrthographicSize = cameraSize;
            SwitchToCamera(_mainCamera, true);
            _isReady = true;
        }

        protected override void UninitSingleton()
        {
            base.UninitSingleton();
            _cameraBrain = null;
            _mainCamera = null;
            _isReady = false;
        }

        public void SwitchToCamera(CinemachineVirtualCameraBase virtualCam, bool force = false)
        {
            if (!IsVaild() && !force) return;
            if ((CinemachineVirtualCameraBase)_cameraBrain.ActiveVirtualCamera == virtualCam) return;
            virtualCam.MoveToTopOfPrioritySubqueue();
            _curCam = virtualCam;
        }

        public void PlayCamShake()
        {
            var cis = virCamObj.GetComponent<CinemachineImpulseSource>();
            if (cis == null) return;
            cis.GenerateImpulse();
        }

        public void LookToObj(GameObject obj)
        {
            if (!IsVaild() || _curCam == null) return;
            _curCam.Follow = obj.transform;
            _curCam.LookAt = obj.transform;
        }

        private bool IsVaild()
        {
            if (_cameraBrain == null || _mainCamera == null) return false;
            if (!_isReady) return false;
            return true;
        }

        private void Update()
        {
            if (PkGameTime.IsPause) return;
            Rescaling();
            HandleInput();
        }

        private void FixedUpdate()
        {
            if (PkGameTime.IsPause) return;
            Move();
        }

        private void Rescaling()
        {
            if ((CinemachineVirtualCameraBase)_cameraBrain.ActiveVirtualCamera == _mainCamera &&
                Mathf.Abs(Input.mouseScrollDelta.y) > 0f && useZoom)
            {
                var mainCam = _mainCamera as CinemachineVirtualCamera;
                if (mainCam != null)
                {
                    var zoom = mainCam.m_Lens.FieldOfView - Input.mouseScrollDelta.y * zoomMultiplier;
                    mainCam.m_Lens.FieldOfView = Mathf.Clamp(zoom, minZoom, maxZoom);
                }
            }
        }

        private void HandleInput()
        {
            if (!useMove) return;
            _dir = Vector3.zero;
            var dirY = Vector3.zero;
            var dirX = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                dirY = Vector3.up;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                dirY = Vector3.down;
            }

            // if (Input.GetKey(KeyCode.RightArrow))
            // {
            //     dirX = Vector3.right;
            // }
            //
            // if (Input.GetKey(KeyCode.LeftArrow))
            // {
            //     dirX = Vector3.left;
            // }

            _dir = (dirX + dirY).normalized;
        }

        private void Move()
        {
            if (_dir.magnitude != 0)
            {
                IsMove = true;
                virCamObj.transform.position += _dir * (cameraSpeed * PkGameTime.GameTime);
            }
            else
            {
                IsMove = false;
            }
        }
        
        public void SetMoveStatus(bool isMove)
        {
            useMove = isMove;
        }
    }
}