using AssetBundlesClass.Game.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Game.Controller
{
    public class GameController : MonoBehaviour
    {
        private static GameController _shared = default;

        private InputActions _inputActions = default;

        public PlayableCar playableCar { get; private set; }
        
        public static GameController shared
        {
            get
            {
                if (_shared) return _shared;
                GameObject gameObject = new GameObject("GameController");
                return _shared = gameObject.AddComponent<GameController>();
            }
        }
        
        public static void SetCar(PlayableCar car) => shared.playableCar = car;
        
        public void MoveToScene(Scene scene) => SceneManager.MoveGameObjectToScene(gameObject, scene);

        public void EnableMenuInput() => _inputActions.Menu.Enable();

        private void Awake()
        {
            if (_shared) DestroyImmediate(gameObject);
            
            _inputActions = new InputActions();
            _inputActions.Menu.Disable();
            _inputActions.Menu.MenuButton.performed += _ => SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }

        private void OnDestroy()
        {
            _shared = null;
        }
    }
}