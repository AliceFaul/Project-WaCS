using UnityEngine;

namespace _Project.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour, IPlayerMovement
    {
        [Header("Movement Settings")]
        [Tooltip("How fast the character moves")]
        [SerializeField] private float moveSpeed;
        [Tooltip("Use its own gravity value. Defaults is -9.81f")]
        [SerializeField] private float gravity = -9.81f;
        
        // movement
        private CharacterController _controller;
        private Vector3 _velocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();

            if (_controller == null)
            {
                Debug.LogWarning("CharacterController not found");
            }
        }

        public void Move(Vector2 input)
        {
            float targetSpeed = moveSpeed;
            if(input == Vector2.zero) targetSpeed = 0;
            Vector3 inputDirection = new Vector3(input.x, 0, input.y).normalized;
            if (input != Vector2.zero)
            {
                inputDirection = transform.right * input.x + transform.forward * input.y;
            }
            if (_controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            _velocity.y += gravity * Time.deltaTime;
            _controller.Move((inputDirection.normalized * targetSpeed + _velocity) * Time.deltaTime);
        }
    }
}