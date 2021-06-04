using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundlesClass.Shared.Pools;
using Sources.Game.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
        [SerializeField] private PlayableCar _vanPlayableCar = default;
        [SerializeField] private Button _downloadDlc1Button = default;
        [SerializeField] private Button _downloadDlc2Button = default;
        
        [SerializeField] private AssetReference _dlc1AssetReference = default;
        [SerializeField] private AssetReference _dlc2AssetReference = default;

        private PlayableCar[] _availableCars = default;

        private int _selectedCarIndex = default;

        private void Awake()
        {
            _playButton.onClick.AddListener(PlayAction);
            _downloadDlc1Button.onClick.AddListener(DownloadDlc1Action);
            _downloadDlc2Button.onClick.AddListener(DownloadDlc2Action);

            _slider.wholeNumbers = true;
            _slider.onValueChanged.AddListener(SelectedCarAction);

            InitializeAvailableCars();
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveAllListeners();
            if (_downloadDlc1Button) _downloadDlc1Button.onClick.RemoveAllListeners();
            if (_downloadDlc2Button) _downloadDlc2Button.onClick.RemoveAllListeners();
            _slider.onValueChanged.RemoveAllListeners();
            
            _dlc1AssetReference.ReleaseAsset();
            _dlc2AssetReference.ReleaseAsset();
        }

        private void InitializeAvailableCars()
        {
            ListPool<PlayableCar> availableCars = ListPool<PlayableCar>.Rent();
            availableCars.Add(_vanPlayableCar);
            CheckDlc1(availableCars);
        }

        private void CheckDlc1(ListPool<PlayableCar> availableCars)
        {
            _dlc1AssetReference.LoadAssetAsync<PlayableCar>().Completed += value =>
            {
                if (value.OperationException != null)
                {
#if ENABLE_DEBUG_LOGS
                    Debug.LogError(value.OperationException);
#endif
                    CheckDlc2(availableCars);
                    return;
                }
                if (value.Status == AsyncOperationStatus.Succeeded && value.Result) 
                    availableCars.Add(value.Result);
                CheckDlc2(availableCars);
            };
        }

        private void CheckDlc2(ListPool<PlayableCar> availableCars)
        {
            _dlc2AssetReference.LoadAssetAsync<AvailableCars>().Completed += value =>
            {
                if (value.OperationException != null)
                {
#if ENABLE_DEBUG_LOGS
                    Debug.LogError(value.OperationException);
#endif
                    CarsInitializationCompleted(availableCars);
                    return;
                }
                if (value.Status == AsyncOperationStatus.Succeeded && value.Result)
                    availableCars.AddRange(value.Result.cars);
                CarsInitializationCompleted(availableCars);
            };
        }

        private void CarsInitializationCompleted(ListPool<PlayableCar> cars)
        {
            _availableCars = cars.ToArray();
            _slider.maxValue = _availableCars.Length - 1;
            _slider.value = _selectedCarIndex;
            SelectedCarAction(_selectedCarIndex);
            cars.Dispose();
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

        private void DownloadDlc1Action()
        {
            _dlc1AssetReference.LoadAssetAsync<PlayableCar>();
        }

        private void DownloadDlc2Action()
        {
            
        }
    }
}