using Sources.Shared.Models;
using UnityEngine;

namespace AssetBundlesClass.Game.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class CameraBehaviour : MonoBehaviour, ICameraInputListener
    {
        public static CameraBehaviour shared { get; private set; } = default;

        [SerializeField] private Vector3 _distanceFromTarget = new Vector3(0F, 1F, -1F);
        [SerializeField] private MinMax _cameraVerticalLock = new MinMax(-90F, 90F);

        private CameraInput _input = default;
        private Transform _transform = default;
        private Vector3 _cameraRotation = default;

        public Transform target { get; set; } = default;

        private void Awake()
        {
            if (shared != null)
            { 
                DestroyImmediate(gameObject);
                return;
            }

            _transform = transform;

            shared = this;

            _input = new CameraInput
            {
                listener = this
            };
        }

        private void OnEnable() => _input.EnableInputs();
        private void OnDisable() => _input.DisableInputs();

        private void LateUpdate()
        {
            if (!target) return;

            Vector3 targetPosition = target.position;
            Quaternion targetRotation = target.rotation;
            Quaternion cameraRotation = Quaternion.Euler(_cameraRotation);

            _transform.position = targetPosition + targetRotation * cameraRotation * _distanceFromTarget;
            _transform.LookAt(targetPosition);
        }

        #region ICameraInputListener

        void ICameraInputListener.MouseHorizontalUpdate(float x) => _cameraRotation.y += x;
        void ICameraInputListener.MouseVerticalUpdate(float y) => _cameraRotation.x = _cameraVerticalLock.Lock(_cameraRotation.x - y);

        #endregion
    }
}
