using UnityEngine;

namespace AssetBundlesClass.Game.Vehicles
{
    public enum TorqueDirection
    {
        x,
        y,
        z,
    }

    public static class TorqueDirectionExt
    {
        public static Vector3 ToDirection(this TorqueDirection direction) => direction switch
        {
            TorqueDirection.x => Vector3.right,
            TorqueDirection.y => Vector3.up,
            TorqueDirection.z => Vector3.forward,
            _ => throw new System.ArgumentOutOfRangeException(nameof(direction), direction.ToString())
        };
    }
}