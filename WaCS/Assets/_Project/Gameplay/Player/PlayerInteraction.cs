using UnityEngine;

namespace _Project.Gameplay.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [Tooltip("Mouse position used to interact with the player")]
        [SerializeField] private new Camera camera;
        [Tooltip("Range for interaction.")]
        [SerializeField] private float interactionRange = 2f;
        [Tooltip("Layers for interaction.")]
        [SerializeField] private LayerMask interactionLayer;
        
        private IInteractable _currentInteractable;
        private PlayerContext _playerContext;
        
        public void Init(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        private void Update()
        {
            DetectInteraction();
            if (_playerContext != null)
            {
                if (_playerContext.Input.Interact && _currentInteractable != null)
                {
                    if (_currentInteractable.CanInteract(_playerContext))
                    {
                        _currentInteractable.Interact(_playerContext);
                    }
                }
            }
            else
            {
                Debug.LogWarning("No player context implement");
            }
        }
        
        // note: refactor detect by mouse
        private void DetectInteraction()
        {
            _currentInteractable = null;
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, interactionRange, interactionLayer))
            {
                hit.collider.TryGetComponent(out _currentInteractable);
            }
        }
    }
}