using System.Collections;
using Sources.Game.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AssetBundlesClass.Game.MainMenu
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        [SerializeField] private Button _playButton = default;
        [SerializeField] private Slider _slider = default;
        [SerializeField] private TMP_Text _carNameLabel = default;

        [SerializeField] private Transform _renderRootTransform = default;
        [SerializeField] private GameObject _currentRenderedCarGameObject = default;
        [SerializeField] private AvailableCars _availableCars = default;
        
        private int _selectedCarIndex = default;

        private void Awake()
        {
            _playButton.onClick.AddListener(PlayAction);

            _slider.wholeNumbers = true;
            _slider.maxValue = _availableCars.length - 1; 
            _slider.onValueChanged.AddListener(SelectedCarAction);
            _slider.value = _selectedCarIndex;
            SelectedCarAction(_selectedCarIndex);
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveAllListeners();
            _slider.onValueChanged.RemoveAllListeners();
        }

        private void PlayAction()
        {
            GameController.SetCar(_availableCars[_selectedCarIndex]);
            StartGameSceneAsync();
        }

        private void SelectedCarAction(float selectedIndex)
        {
            _selectedCarIndex = (int) selectedIndex;
            PlayableCar car = _availableCars[_selectedCarIndex];
            _carNameLabel.text = car.name;
            
            if (_currentRenderedCarGameObject) DestroyImmediate(_currentRenderedCarGameObject);

            GameObject carGameObject = Instantiate(car.carPrefab, _renderRootTransform);
            carGameObject.transform.localPosition = Vector3.zero;
            carGameObject.transform.localRotation = Quaternion.identity;
            Rigidbody carRigidbody = carGameObject.GetComponent<Rigidbody>();
            if (carRigidbody)
            {
                carRigidbody.useGravity = false;
                carRigidbody.isKinematic = true;
            }

            _currentRenderedCarGameObject = carGameObject;
        }

        private void StartGameSceneAsync()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
            StartCoroutine(HandleSceneLoading(operation));
        }

        private IEnumerator HandleSceneLoading(AsyncOperation operation)
        {
            do yield return new WaitForEndOfFrame();
            while (!operation.isDone);
            
            Scene gameScene = SceneManager.GetSceneByName("GameScene");
            SceneManager.SetActiveScene(gameScene);

            GameController.shared.MoveToScene(gameScene);

            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
    }
}