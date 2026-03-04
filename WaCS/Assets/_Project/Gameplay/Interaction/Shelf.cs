using _Project.Gameplay.Player;
using _Project.Systems.Game;
using _Project.UI.Screens;
using UnityEngine;

namespace _Project.Gameplay.Interaction
{
    public class Shelf : MonoBehaviour, IInteractable, ISecondaryInteractable
    {
        [SerializeField] private ShelfController _controller;
        [SerializeField] private PriceUI priceUI;

        public bool CanInteract(PlayerContext context)
        {
            return context.Hotbar.HasEquippedItem();
        }

        // Secondary interact is only available if the player is looking at a shelf point that has items on it
        public bool CanSecondaryInteract(PlayerContext context)
        {
            ShelfPoint point = GetLookedShelfPoint();
            return point != null && point.HasItems;
        }

        public string GetPrompt()
        {
            return "Place Item in shelf";
        }

        public string GetSecondaryPrompt()
        {
            return "Set price for this shelf";
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

        // Secondary interact allows the player to set the price of the items on the shelf point they are looking at (if any)
        public void SecondaryInteract(PlayerContext context)
        {
            ShelfPoint point = GetLookedShelfPoint();
            if (point == null || !point.HasItems)
            {
                Debug.LogWarning("Player tried to set price on a shelf point that doesn't have items");
                return;
            }
            priceUI.Open(point);
        }

        // Helper method to get the shelf point the player is currently looking at (if any)
        private ShelfPoint GetLookedShelfPoint()
        {
            Ray ray = new Ray(Camera.main.transform.position, 
                Camera.main.transform.forward);
            if(Physics.Raycast(ray, out RaycastHit hit, 3f))
            {
                return hit.collider.GetComponentInParent<ShelfPoint>();
            }
            return null;
        }
    }
}
