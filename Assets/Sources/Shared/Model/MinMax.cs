using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Sources.Shared.Models
{
    [Serializable]
    public class MinMax
    {
        [UsedImplicitly] public float min = default;
        [UsedImplicitly] public float max = default;

        public MinMax(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Lock(float current) => Mathf.Clamp(current, min, max);
    }
}