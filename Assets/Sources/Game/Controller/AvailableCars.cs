using UnityEngine;

namespace Sources.Game.Controller
{
    [CreateAssetMenu(menuName = "Cars/Available cars list")]
    public class AvailableCars : ScriptableObject
    {
        [SerializeField] private PlayableCar[] _cars = default;

        public PlayableCar[] cars => _cars;
    }
}