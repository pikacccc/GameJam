using Cinemachine;
using Runtime.PKGameCore.PKTools;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Runtime.PKGameCore.PKCamera
{
    public class CameraManager : SingletonBehaviour<CameraManager>
    {
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
        public float originalOrthoSize = 10f;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            _cameraBrain = camBrainObj.GetComponent<CinemachineBrain>();
            _mainCamera = virCamObj.GetComponent<CinemachineVirtualCameraBase>();
            ((CinemachineVirtualCamera)_mainCamera).m_Lens.OrthographicSize = originalOrthoSize;
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
    }
}