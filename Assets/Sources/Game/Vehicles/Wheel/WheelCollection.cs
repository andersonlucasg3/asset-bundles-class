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

        public void Accelerate(float acceleration)
        {

        }

        public void Steer(float steering)
        {

        }
    }
}