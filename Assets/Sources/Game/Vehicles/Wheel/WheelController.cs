using System;
using UnityEngine;

namespace AssetBundlesClass.Game.Vehicles.Wheel
{
    [Serializable]
    public class WheelController
    {
        [SerializeField] private Transform _transform = default;
        [SerializeField] private WheelCollider _wheelCollider = default;

        public WheelController(Transform transform)
        {
            _wheelCollider = transform.GetComponent<WheelCollider>();
            _transform = transform.GetChild(0);
        }

        public void Update()
        {
            _wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);

            _transform.position = position;
            _transform.rotation = rotation;
        }

        public void Accelerate(float acceleration)
        {
            if (acceleration >= 0) _wheelCollider.motorTorque = acceleration;
            else
            {
                _wheelCollider.motorTorque = 0F;
                _wheelCollider.brakeTorque = acceleration;
            }
        }

        public void Steer(float steeringAngle)
        {
            _wheelCollider.steerAngle = steeringAngle;
        }
    }
}
