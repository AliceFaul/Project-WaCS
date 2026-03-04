using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Systems.Game;
using _Project.Core.Event;
using System;
using Project.Systems.SaveLoad;

namespace _Project.Core.SceneManagement
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private ItemDictionary itemDictionary;
        [SerializeField] private bool loadExistingSave = false;
        [SerializeField] private string saveFileName = "DefaultSave";
        [SerializeField] private AudioDictionary audioDictionary; 

        private readonly List<IManager> _managers = new List<IManager>();
        private static bool _isInitialized = false;

        private async void Awake()
        {
            if(_isInitialized)
            {
                Debug.LogWarning("[Bootstrapper] already initialized, skipping...");
                Destroy(gameObject);
                return;
            }

            _isInitialized = true;
            DontDestroyOnLoad(this.gameObject);

            try
            {
                ItemDictionary.SetInstance(itemDictionary);
                RegisterManager();
                await InitManager();

                await SceneLoader.Instance.LoadSceneGroup(0);
                await Task.Yield(); // đảm bảo tất cả các manager đã hoàn thành init trước khi load save

                if (loadExistingSave)
                {
                    SaveLoadService.Instance.LoadGame(saveFileName);
                }
                else
                {
                    SaveLoadService.Instance.NewGame();
                }
            }
            catch(Exception e)
            {
                Debug.LogError($"[Bootstrapper] init failed: {e}");
            }
        }

        private void RegisterManager()
        {
            var shelfSystem = new ShelfService();
            Register(shelfSystem);
            var eventManager = EventManager.Instance;
            Register(eventManager);
            var checkoutSystem = new CheckoutSystem(eventManager);
            Register(checkoutSystem);
            var economySystem = new EconomySystem(eventManager);
            Register(economySystem);
            var audioObj = new GameObject("AudioService");
            var audioService = audioObj.AddComponent<AudioService>();
            audioService.SetDictionary(audioDictionary);
            Register(audioService);
        }

        private async Task InitManager()
        {
            foreach(var manager in _managers)
            {
                var success = await manager.InitAsync();
                if(!success)
                {
                    Debug.LogError($"[Bootstrapper] failed to init {manager.GetType().Name}");
                    break;
                }
            }
            Debug.Log("[Bootstrapper] all managers initialized successfully");
        }

        private void Register(IManager manager)
        {
            ServiceRegistry.Register(manager);
            _managers.Add(manager);
            if(manager is ISaveable saveable)
            {
                SaveLoadService.Instance.RegisterSaveData(saveable);
            }
        }
    }
}