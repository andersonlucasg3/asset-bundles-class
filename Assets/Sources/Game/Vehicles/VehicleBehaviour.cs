using AssetBundlesClass.Game.InputSystem;
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
        [SerializeField] private TorqueDirection _torqueDirection = default;

        private Transform _transform = default;
        private Rigidbody _rigidbody = default;
        private CarInput _input = default;

        private float _currentAcceleration = default;
        private float _currentSteering = default;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();

            _input = new CarInput();
            _input.AddListener(this);
        }

        private void Start() => _input.EnableInputs();

        private void OnDisable() => _input.DisableInputs();

        private void OnDestroy() => _input.RemoveListener(this);

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            float acceleration = _currentAcceleration;
            float steering = _currentSteering;

            _rigidbody.AddTorque(_torqueDirection.ToDirection() * _engineTorque * _currentAcceleration * deltaTime, ForceMode.Force);

            switch (_traction)
            {
                case VehicleTraction.front:
                    _frontWheels.Accelerate(acceleration);
                    // breaking
                    if (acceleration < 0) _rearWheels.Accelerate(acceleration);
                    break;
                case VehicleTraction.rear:
                    _rearWheels.Accelerate(acceleration);
                    // breaking
                    if (acceleration < 0) _frontWheels.Accelerate(acceleration);
                    break;
                case VehicleTraction.all:
                    _frontWheels.Accelerate(acceleration);
                    _rearWheels.Accelerate(acceleration);
                    break;
            }

            _frontWheels.Steer(steering * _maxSteeringAngle);
        }

        #region ICarInputListener

        void ICarInputListener.AccelerationUpdate(float acceleration) => _currentAcceleration = acceleration;

        void ICarInputListener.SteeringUpdate(float steering) => _currentSteering = steering;

        #endregion
    }
}