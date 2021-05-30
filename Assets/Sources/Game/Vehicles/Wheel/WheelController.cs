using System;
using UnityEngine;

namespace AssetBundlesClass.Game.Vehicles.Wheel
{
    [Serializable]
    public class WheelController
    {
        [SerializeField] private Transform _transform;

        public WheelController(Transform transform)
        {
            _transform = transform;
        }
    }
}
