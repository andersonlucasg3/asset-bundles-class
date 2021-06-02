using AssetBundlesClass.Game.Vehicles;
using UnityEngine;

namespace Sources.Game.Controller
{
    [CreateAssetMenu(menuName = "Cars/New Car")]
    public class PlayableCar : ScriptableObject
    {
        [SerializeField] private GameObject _carPrefab = default;

        public GameObject carPrefab => _carPrefab;
    }
}