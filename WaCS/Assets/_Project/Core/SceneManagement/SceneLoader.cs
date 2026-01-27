using System;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace _Project.Core.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Image loadingBar;
        [SerializeField] private Canvas loadingCanvas;
        [SerializeField] private CanvasGroup loadingCanvasGroup;
        [SerializeField] private Camera loadingCamera;
        [SerializeField] private float fillSpeed;
        [SerializeField] private float fadeDuration;
        [SerializeField] private SceneGroup[] sceneGroups;
        
        private float _targetProgress;
        private bool _isLoading;
        
        private readonly SceneGroupController _sceneGroupController =  new SceneGroupController();

        private void Awake()
        {
            _sceneGroupController.OnSceneLoaded += sceneName => Debug.Log($"Loaded Scene: {sceneName}");
            _sceneGroupController.OnSceneUnloaded += sceneName => Debug.Log($"Unloaded Scene: {sceneName}");
            _sceneGroupController.OnSceneGroupLoaded += () => Debug.Log("Loaded Scene Group");
            // NOTE: load scene group xong sẽ fade out 
            _sceneGroupController.OnSceneGroupLoaded += async() => await FadeOut();
        }
        
        private void Update()
        {
            if (!_isLoading) return;
            float currentFillAmount = loadingBar.fillAmount;
            float progressDifferent = Mathf.Abs(currentFillAmount - _targetProgress);
            float dynamicSpeed = progressDifferent * fillSpeed;
            loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, _targetProgress, dynamicSpeed * Time.deltaTime);
        }

        public async Task LoadSceneGroup(int index)
        {
            loadingBar.fillAmount = 0.0f;
            _targetProgress = 1f;
            if (index < 0 || index >= sceneGroups.Length)
            {
                Debug.LogWarning($"Index out of group: {index}");
                return;
            }
            LoadingProgress progress = new LoadingProgress();
            progress.Progressed += t => _targetProgress = Mathf.Max(t, _targetProgress);
            await FadeIn();
            await _sceneGroupController.LoadScenes(sceneGroups[index], progress);
        }

        private void EnableLoadingScreen(bool enable = true)
        {
            _isLoading = enable;
            loadingCanvas.enabled = enable;
            loadingCamera.enabled = enable;
        }

        #region Fade Loading Screen
        
        private async Task Fade(float target)
        {
            float start = loadingCanvasGroup.alpha;
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                loadingCanvasGroup.alpha = Mathf.Lerp(start, target, t / fadeDuration);
                await Task.Yield();
            }
            loadingCanvasGroup.alpha = target;
        }

        private async Task FadeIn()
        {
            EnableLoadingScreen();
            await Fade(1f);
        }

        private async Task FadeOut()
        {
            await Fade(0f);
            EnableLoadingScreen(false);
        }
        
        #endregion
    }

    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> Progressed;
        private const float Ratio = 1f;

        public void Report(float value)
        {
            Progressed?.Invoke(value / Ratio);
        }
    }
}