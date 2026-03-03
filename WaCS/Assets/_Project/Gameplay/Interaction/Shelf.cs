using _Project.Gameplay.Player;
using _Project.Systems.Game;
using UnityEngine;

namespace _Project.Gameplay.Interaction
{
    public class Shelf : MonoBehaviour, IInteractable
    {
        [SerializeField] private ShelfController _controller;

        public bool CanInteract(PlayerContext context)
        {
            return context.Hotbar.HasEquippedItem();
        }

        public string GetPrompt()
        {
            return "Place Item in shelf";
        }

        public void Interact(PlayerContext context)
        {
            if(!context.Hotbar.HasEquippedItem())
            {
                Debug.LogWarning("Player tried to interact with shelf without an equipped item");
                return;
            }
            ItemData item = context.Hotbar.GetEquippedItem();
            bool placed = _controller.TryPlaceItem(item);
            if(!placed)
            {
                Debug.LogWarning("Could not place item on shelf: shelf is full or item type doesn't match");
                return;
            }
            // If we successfully placed the item on the shelf, remove it from the player's inventory
            context.Inventory.RemoveItem(item);
            // Note: We don't need to clear the hotbar slot here
            // because the inventory system should trigger an update that will automatically clear the hotbar slot if the item is removed from the inventory
        }
    }
}
