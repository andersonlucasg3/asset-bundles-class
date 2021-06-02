using AssetBundlesClass.Game.Vehicles;
using Sources.Game.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetBundlesClass.Game.KickStart
{
    public class GameKicker : MonoBehaviour
    {
        private void Awake()
        {
            PlayableCar playableCar = GameController.shared.playableCar;
            GameObject carGameObject = Instantiate(playableCar.carPrefab, Vector3.zero, Quaternion.identity);
            if (!carGameObject.TryGetComponent(out VehicleBehaviour vehicleBehaviour))
            {
                SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
                return;
            }
            SceneManager.MoveGameObjectToScene(carGameObject, gameObject.scene);
            GameController.shared.EnableMenuInput();
            vehicleBehaviour.enabled = true;
            DestroyImmediate(gameObject);
        }
    }
}
