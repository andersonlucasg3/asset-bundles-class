using System;
using UnityEngine;

namespace AssetBundlesClass.Game.Vehicles.Wheel
{
    [Serializable]
    public class WheelCollection
    {
        [SerializeField] private WheelController[] _wheels = default;

        public WheelCollection(WheelController[] wheels)
        {
            _wheels = wheels;
        }

        public void Update(float acceleration, float steering = 0)
        {
            for (int index = 0; index < _wheels.Length; index++)
            {
                WheelController wheel = _wheels[index];
                wheel.Accelerate(acceleration);
                wheel.Steer(steering);
                wheel.Update();
            }
        }
    }
}