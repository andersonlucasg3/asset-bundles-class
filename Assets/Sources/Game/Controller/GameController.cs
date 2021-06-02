using UnityEngine;

namespace Sources.Game.Controller
{
    public class GameController : MonoBehaviour
    {
        private static GameController _shared = default;

        private PlayableCar _playableCar = default;
        
        public static GameController shared
        {
            get
            {
                if (_shared) return _shared;
                GameObject gameObject = new GameObject("GameController");
                return _shared = gameObject.AddComponent<GameController>();
            }
        }
        
        public static void SetCar(PlayableCar car) => shared._playableCar = car;

        private void Awake()
        {
            if (_shared) DestroyImmediate(gameObject);
        }

        private void OnDestroy()
        {
            _shared = null;
        }
    }
}