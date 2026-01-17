using UnityEngine;

namespace _Project.Gameplay.Input
{
    public class InputSystem : MonoBehaviour, IPlayerInput
    {
        private PlayerInputSystem _actions;
        
        public Vector2 Move  { get; set; }
        public Vector2 Look { get; set; }
        public bool Interact { get; private set; }

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
            
            _actions.Player.Interact.performed += _ => Interact = true;
        }

        private void OnDisable()
        {
            _actions.Disable();
        }

        private void LateUpdate()
        {
            Interact = false;
        }
    }
}