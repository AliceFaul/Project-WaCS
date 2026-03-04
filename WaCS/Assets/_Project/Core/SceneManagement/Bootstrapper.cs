using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Systems.Game;
using _Project.Core.Event;
using System;

namespace _Project.Core.SceneManagement
{
    public class Bootstrapper : MonoBehaviour
    {
        private readonly List<IManager> _managers = new List<IManager>();

        private async void Awake()
        {
            try
            {
                RegisterManager();
                await InitManager();
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