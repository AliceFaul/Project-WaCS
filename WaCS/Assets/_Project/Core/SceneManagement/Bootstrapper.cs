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

                if(loadExistingSave)
                {
                    SaveLoadService.Instance.LoadGame(saveFileName);
                }
                else
                {
                    SaveLoadService.Instance.NewGame();
                }
                await SceneLoader.Instance.LoadSceneGroup(0);
            }
            catch(Exception e)
            {
                Debug.LogError($"[Bootstrapper] init failed: {e}");
            }
        }

        private void RegisterManager()
        {
            var eventManager = EventManager.Instance;
            Register(eventManager);
            var checkoutSystem = new CheckoutSystem(eventManager);
            Register(checkoutSystem);
            var economySystem = new EconomySystem(eventManager);
            Register(economySystem);
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
        }

        private void Register(IManager manager)
        {
            ServiceRegistry.Register(manager);
            _managers.Add(manager);
        }
    }
}