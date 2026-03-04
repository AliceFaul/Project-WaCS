using UnityEngine;

namespace _Project.Gameplay.Input
{
    public class InputSystem : MonoBehaviour, IPlayerInput
    {
        private PlayerInputSystem _actions;
        
        public Vector2 Move  { get; set; }
        public Vector2 Look { get; set; }
        public bool Interact { get; private set; }
        public bool SecondaryInteract { get; private set; }
        public bool Inventory { get; private set; }
        public bool Escape { get; private set; }

        public bool HotbarNext { get; private set; }
        public bool HotbarPrevious { get; private set; }

        public int HotbarNumberPressed { get; private set; } = -1;

        private void Awake()
        {
            _actions = new PlayerInputSystem();
        }

        private void OnEnable()
        {
            _actions.Enable();
            
            _actions.Player.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();
            _actions.Player.Move.canceled += _ => Move = Vector2.zero;
            
            _actions.Player.Look.performed += ctx => Look = ctx.ReadValue<Vector2>();
            _actions.Player.Look.canceled += _ => Look = Vector2.zero;
            
            _actions.Player.Interact.performed += ctx => Interact = true;
            _actions.Player.SecondaryInteract.performed += ctx => SecondaryInteract = true;

            _actions.Player.Inventory.performed += ctx => Inventory = true;

            _actions.Player.Escape.performed += ctx => Escape = true;


            _actions.Player.HotbarNext.performed += ctx => HotbarNext = true;
            _actions.Player.HotbarPrev.performed += ctx => HotbarPrevious = true;

            _actions.Player.HotbarNumber1.performed += ctx => HotbarNumberPressed = 0;
            _actions.Player.HotbarNumber2.performed += ctx => HotbarNumberPressed = 1;
            _actions.Player.HotbarNumber3.performed += ctx => HotbarNumberPressed = 2;
            _actions.Player.HotbarNumber4.performed += ctx => HotbarNumberPressed = 3;
            _actions.Player.HotbarNumber5.performed += ctx => HotbarNumberPressed = 4;
            _actions.Player.HotbarNumber6.performed += ctx => HotbarNumberPressed = 5;
            _actions.Player.HotbarNumber7.performed += ctx => HotbarNumberPressed = 6;
            _actions.Player.HotbarNumber8.performed += ctx => HotbarNumberPressed = 7;
            _actions.Player.HotbarNumber9.performed += ctx => HotbarNumberPressed = 8;
        }

        private void OnDisable()
        {
            _actions.Disable();
        }

        private void LateUpdate()
        {
            // Reset one-time actions after they've been processed
            Interact = false;
            SecondaryInteract = false;
            Inventory = false;
            Escape = false;

            HotbarNext = false;
            HotbarPrevious = false;
            HotbarNumberPressed = -1;
        }
    }
}