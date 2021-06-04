using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundlesClass.Game.AssetBundlesSystem;
using AssetBundlesClass.Shared.Pools;
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
        [SerializeField] private PlayableCar _vanPlayableCar = default;
        [SerializeField] private Button _downloadDlc1Button = default;
        [SerializeField] private Button _downloadDlc2Button = default;

        [SerializeField] private string _assetBundlesUrl = default;
        
        private PlayableCar[] _availableCars = default;

        private AssetBundlesLoader _assetBundlesLoader;
        
        private int _selectedCarIndex = default;

        private void Awake()
        {
            AssetBundlesLoader.Initialize(_assetBundlesUrl);
            _assetBundlesLoader = AssetBundlesLoader.shared;
            
            _playButton.onClick.AddListener(PlayAction);
            _downloadDlc1Button.onClick.AddListener(DownloadDlc1Action);
            _downloadDlc2Button.onClick.AddListener(DownloadDlc2Action);

            _slider.wholeNumbers = true;
            _slider.onValueChanged.AddListener(SelectedCarAction);

            StartCoroutine(InitializeAvailableCars());
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveAllListeners();
            if (_downloadDlc1Button) _downloadDlc1Button.onClick.RemoveAllListeners();
            if (_downloadDlc2Button) _downloadDlc2Button.onClick.RemoveAllListeners();
            _slider.onValueChanged.RemoveAllListeners();
        }

        private IEnumerator InitializeAvailableCars()
        {
            using ListPool<PlayableCar> availableCars = ListPool<PlayableCar>.Rent();
            availableCars.Add(_vanPlayableCar);
            yield return CheckDlc1(availableCars);
        }

        private IEnumerator CheckDlc1(List<PlayableCar> availableCars)
        {
            if (!_assetBundlesLoader.HasCache("dlc1"))
            {
                yield return CheckDlc2(availableCars);
                yield break;
            }
            
            Destroy(_downloadDlc1Button.gameObject);

            IEnumerator CarLoaded(PlayableCar car)
            {
                availableCars.Add(car);
                yield return CheckDlc2(availableCars);
            }
            yield return _assetBundlesLoader.Load<PlayableCar>("dlc1", "Mustang.asset", CarLoaded);
        }

        private IEnumerator CheckDlc2(List<PlayableCar> availableCars)
        {
            if (!_assetBundlesLoader.HasCache("dlc2"))
            {
                CarsInitializationCompleted(availableCars.ToArray());
                yield break;
            }
            
            Destroy(_downloadDlc2Button.gameObject);

            IEnumerator CarsLoaded(PlayableCar[] cars)
            {
                availableCars.AddRange(cars);
                yield return new WaitForEndOfFrame();
                CarsInitializationCompleted(availableCars.ToArray());
            }
            yield return _assetBundlesLoader.LoadMany<PlayableCar>("dlc2", new[] {"Cybertruck.asset", "Dodge Challenger.asset"}, CarsLoaded);
        }

        private void CarsInitializationCompleted(PlayableCar[] cars)
        {
            _availableCars = cars;
            _slider.maxValue = _availableCars.Length - 1;
            _slider.value = _selectedCarIndex;
            SelectedCarAction(_selectedCarIndex);
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

        private void DownloadDlc1Action() => StartCoroutine(_assetBundlesLoader.LoadAssetBundle("dlc1", OnCompleteBuyingAssetBundle));

        private void DownloadDlc2Action() => StartCoroutine(_assetBundlesLoader.LoadAssetBundle("dlc2", OnCompleteBuyingAssetBundle));

        private static IEnumerator OnCompleteBuyingAssetBundle(bool success)
        {
            if (!success) throw new ArgumentException("Should be true", nameof(success));
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
            yield break;
        }
    }
}