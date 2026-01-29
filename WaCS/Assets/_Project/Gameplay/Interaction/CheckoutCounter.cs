using _Project.Gameplay.Player;
using _Project.Systems.Game;
using UnityEngine;

namespace _Project.Gameplay.Interaction
{
    public class CheckoutCounter : MonoBehaviour, IInteractable
    {
        public void Interact(PlayerContext context)
        {
            
        }

        public bool CanInteract(PlayerContext context)
        {
            return true;
        }

        public string GetPrompt()
        {
            return "Press E to Checkout";
        }
    }
}