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
        [SerializeField] private Image _carThumbnail = default;

        [SerializeField] private AvailableCars _availableCars = default;

        private int _selectedCarIndex = default;

        private void Awake()
        {
            _playButton.onClick.AddListener(PlayAction);

            _slider.wholeNumbers = true;
            _slider.maxValue = _availableCars.length; 
            _slider.onValueChanged.AddListener(SelectedCarAction);
            _slider.value = _selectedCarIndex;
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
            
            const int squareSize = 300;
            Vector2 pivot = new Vector2(.5F, .5F);
            Rect rect = new Rect(0, 0, squareSize, squareSize);
            Texture2D generatedTexture = RuntimePreviewGenerator.GenerateModelPreview(car.carPrefab.transform, squareSize, squareSize);
            
            if (_carThumbnail.sprite) Destroy(_carThumbnail.sprite);
            
            _carThumbnail.sprite = Sprite.Create(generatedTexture, rect, pivot);
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

            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
    }
}