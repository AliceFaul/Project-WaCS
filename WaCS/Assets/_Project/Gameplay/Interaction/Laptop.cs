using _Project.Gameplay.Player;
using UnityEngine;

namespace _Project.Gameplay.Interaction
{
    public class Laptop : MonoBehaviour, IInteractable
    {
        public void Interact(PlayerContext context)
        {
            Debug.Log($"Open Shop");
        }

        public bool CanInteract(PlayerContext context)
        {
            return true;
        }

        public string GetPrompt()
        {
            return $"Press E to Open Shop";
        }
    }
}