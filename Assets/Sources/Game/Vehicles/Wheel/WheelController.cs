using System;
using UnityEngine;

namespace AssetBundlesClass.Game.Vehicles.Wheel
{
    [Serializable]
    public class WheelController
    {
        [SerializeField] private Transform _transform = default;
        [SerializeField] private WheelCollider _wheelCollider = default;
        [SerializeField] private Rigidbody _attachedRigidbody = default;

        private float _currentSpeed = default;
        private bool _isInReverse = false;
        
        public WheelController(Transform transform)
        {
            _wheelCollider = transform.GetComponent<WheelCollider>();
            _transform = transform.GetChild(0);
            _attachedRigidbody = _wheelCollider.attachedRigidbody;
        }

        public void Update()
        {
            _wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);

            _transform.position = position;
            _transform.rotation = rotation;

            _currentSpeed = _attachedRigidbody.velocity.magnitude * 3.6F; // m/s to km/h
        }

        public void Accelerate(float acceleration)
        {
            if (_currentSpeed < 0.001F) _isInReverse = acceleration < 0F;

            _wheelCollider.motorTorque = GetMotorTorque(acceleration);
            _wheelCollider.brakeTorque = GetBrakeTorque(acceleration);
        }

        public void Steer(float steeringAngle) => _wheelCollider.steerAngle = steeringAngle;

        private float GetMotorTorque(float acceleration)
        {
            return _isInReverse switch
            {
                false when acceleration > 0F => acceleration,
                false when acceleration < 0F => 0F,
                true when acceleration < 0F => acceleration,
                true when acceleration > 0F => 0F,
                _ => 0F
            };
        }

        private float GetBrakeTorque(float acceleration)
        {
            return _isInReverse switch
            {
                false when acceleration > 0F => 0F,
                false when acceleration < 0F => -acceleration,
                true when acceleration < 0F => 0F,
                true when acceleration > 0F => acceleration,
                _ => acceleration
            };
        }
    }
}
