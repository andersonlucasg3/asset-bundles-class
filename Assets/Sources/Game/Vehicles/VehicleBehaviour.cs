using AssetBundlesClass.Game.Cameras;
using AssetBundlesClass.Game.Vehicles.Input;
using AssetBundlesClass.Game.Vehicles.Wheel;
using UnityEngine;

namespace AssetBundlesClass.Game.Vehicles
{
    public class VehicleBehaviour : MonoBehaviour, ICarInputListener
    {
        [SerializeField] private WheelCollection _frontWheels = default;
        [SerializeField] private WheelCollection _rearWheels = default;
        [SerializeField] private VehicleTraction _traction = default;
        [SerializeField] private float _maxSteeringAngle = default;
        [SerializeField] private float _engineTorque = default;

        private Transform _transform = default;
        private CarInput _input = default;

        private float _currentAcceleration = default;
        private float _currentSteering = default;

#if UNITY_EDITOR
        public WheelCollection frontWheels { set => _frontWheels = value; }
        public WheelCollection rearWheels { set => _rearWheels = value; }
#endif

        private void Awake()
        {
            _transform = transform;

            _input = new CarInput();
            _input.AddListener(this);
        }

        private void OnEnable()
        {
            CameraBehaviour.shared.target = _transform;

            _input.EnableInputs();
        }

        private void OnDisable()
        {
            CameraBehaviour.shared.target = null;

            _input.DisableInputs();
        }

        private void OnDestroy() => _input.RemoveListener(this);

        private void Update()
        {
            float acceleration = _currentAcceleration;
            float steering = _currentSteering;

            float finalAcceleration = acceleration * _engineTorque;
            float finalSteering = steering * _maxSteeringAngle;
            switch (_traction)
            {
                case VehicleTraction.front:
                    _frontWheels.Update(finalAcceleration, finalSteering);
                    _rearWheels.Update(0F);
                    break;
                case VehicleTraction.rear:
                    _frontWheels.Update(0F, finalSteering);
                    _rearWheels.Update(finalAcceleration);
                    break;
                case VehicleTraction.all:
                    _frontWheels.Update(finalAcceleration, finalSteering);
                    _rearWheels.Update(finalAcceleration);
                    break;
            }
        }

        #region ICarInputListener

        void ICarInputListener.AccelerationUpdate(float acceleration) => _currentAcceleration = acceleration;

        void ICarInputListener.SteeringUpdate(float steering) => _currentSteering = steering;

        #endregion
    }
}