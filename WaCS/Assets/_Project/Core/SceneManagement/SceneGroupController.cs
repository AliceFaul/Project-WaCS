using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

namespace _Project.Core.SceneManagement
{
    public class SceneGroupController
    {
        public event Action<string> OnSceneLoaded = delegate { };
        public event Action<string> OnSceneUnloaded = delegate { };
        public event Action OnSceneGroupLoaded = delegate { };

        private SceneGroup _sceneGroup;
        
        // NOTE: hàm load scene theo sceneGroup
        public async Task LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes = false)
        {
            // tạo một danh sách các scene sẽ load và unload toàn bộ scene
            _sceneGroup = group; // cập nhật sceneGroup theo group được tham chiếu
            var loadedScenes = new List<string>();
            await UnloadScenes();
            
            // lưu lại toàn bộ scene trong danh sách
            var sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }
            
            // load từng scene trong sceneGroup
            var totalSceneToLoad = _sceneGroup.scenes.Count;
            var operationGroup = new AsyncOperationGroup(totalSceneToLoad);
            for (int i = 0; i < totalSceneToLoad; i++)
            {
                var sceneData = group.scenes[i];
                // bỏ qua các scene đã load (tránh duplicate scene)
                if (!reloadDupScenes && loadedScenes.Contains(sceneData.SceneName)) continue;
                var operation = SceneManager.LoadSceneAsync(sceneData.sceneReference, LoadSceneMode.Additive);
                await Task.Delay(TimeSpan.FromSeconds(2.5f));
                
                operationGroup.Operations.Add(operation);
                OnSceneLoaded?.Invoke(sceneData.SceneName);
            }
            
            // cập nhật tiến độ loading
            while (!operationGroup.IsDone)
            {
                progress?.Report(operationGroup.Progress);
                await Task.Delay(100);
            }
            
            // Set active cho scene trong sceneGroup có type là ActiveScene
            Scene activeScene = SceneManager
                .GetSceneByName(_sceneGroup.FindSceneNameByType(SceneType.ActiveScene));
            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }
            OnSceneGroupLoaded?.Invoke();
        }
        
        // NOTE: hàm được dùng để unload tất cả scene trừ active scene và bootstrapper scene
        // unload scene bằng cách gom thành một danh sách và unload cùng một lúc và giải phóng bộ nhớ
        private async Task UnloadScenes()
        {
            // khởi tạo danh sách các scene cần unload
            var scenes = new List<string>();
            var activeScene = SceneManager.GetActiveScene().name;
            int sceneCount = SceneManager.sceneCount;
            for (int i = sceneCount - 1; i > 0; i--)
            {
                var sceneAt = SceneManager.GetSceneAt(i);
                if (!sceneAt.isLoaded) continue; // bỏ qua nếu scene chưa được load
                var sceneName = sceneAt.name;
                // bỏ qua active scene và scene bootstrapper
                if (sceneName.Equals(activeScene) || sceneName == "Bootstrapper") continue;
                scenes.Add(sceneName);
            }
            
            // unload từng scene trong danh sách
            var operationGroup = new AsyncOperationGroup(scenes.Count);
            foreach (var scene in scenes)
            {
                var operation = SceneManager.UnloadSceneAsync(scene);
                if (operation == null) continue;
                operationGroup.Operations.Add(operation);
                OnSceneUnloaded?.Invoke(scene);
            }
            
            // chờ các scene unload xong rồi giải phóng bộ nhớ
            while (!operationGroup.IsDone)
            {
                await Task.Delay(100);
            }
            await Resources.UnloadUnusedAssets();
        }
    }

    public readonly struct AsyncOperationGroup
    {
        public readonly List<AsyncOperation> Operations;
        
        public float Progress => Operations.Count == 0 ? 0 : Operations.Average(op => op.progress);
        public bool IsDone => Operations.All(op => op.isDone);

        public AsyncOperationGroup(int initialCapacity)
        {
            Operations = new List<AsyncOperation>(initialCapacity);
        }
    }
}