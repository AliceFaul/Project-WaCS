using UnityEngine;
using System;
using Project.Systems.Game;

namespace _Project.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }
        public PlayerContext Context { get; private set; }
        
        [SerializeField] private PlayerInteraction interaction;

        public event Action<bool> OnInventoryToggled;
        private bool _inventoryOpen = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Multiple instances of PlayerController detected. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

            Context = new PlayerContext(this);
            ServiceRegistry.Register(Context);
            Context.StateMachine.ChangeState(new  PlayerIdleState(this));
            interaction.Init(Context);
        }

        private void Update()
        {
            Context.Movement.Move(Context.Input.Move);
            Context.StateMachine.UpdateState();
            Context.Hotbar.BindInput(Context.Input);
            if(Context.Input.Inventory)
            {
                _inventoryOpen =  !_inventoryOpen;
                OnInventoryToggled?.Invoke(_inventoryOpen);
            }
        }
    }
}