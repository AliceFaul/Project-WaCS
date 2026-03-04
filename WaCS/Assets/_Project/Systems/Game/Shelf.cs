using UnityEngine;
using System.Collections.Generic;

namespace _Project.Systems.Game
{
    // Represents a single slot on the shelf, which can hold a specific item and track its stock level.
    [System.Serializable]
    public class ShelfSlot
    {
        public ItemData item;
        public int stock;
    }

    // Represents a customer's shopping cart, allowing them to add items they intend to purchase.
    public class CustomerCart
    {
        private List<ItemData> items = new List<ItemData>();

        public void Add(ItemData item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public IReadOnlyList<ItemData> Item => items.AsReadOnly();
    }

    public class Shelf : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] public List<ShelfSlot> slots = new List<ShelfSlot>();
        [SerializeField] private int maxStockPerSlot = 10;
        [SerializeField] private int maxSlotCount = 5;

        // Returns true if the shelf has at least one unit of the specified item in stock
        public bool HasStock(string itemID)
        {
            var slot = slots.Find(s => s.item.ItemID == itemID);
            return slot != null && slot.stock > 0;
        }

        // Returns true if item was placed successfully, false if no slot available or invalid input
        public bool PlaceItem(ItemData item, int amount = 1)
        {
            if(item == null || amount <= 0) return false;
            // Try to find existing slot for this item
            var slot = slots.Find(s => s.item.ItemID == item.ItemID);
            // If found, restock it. If not, try to add a new slot if we have capacity.
            if(slot != null)
            {
                // Restock existing slot, but don't exceed max stock
                int newStock = Mathf.Min(slot.stock + amount, maxStockPerSlot);
                slot.stock = newStock;
                return true;
            }
            else
            {
                // No existing slot, try to add new one if we have capacity
                if(slots.Count < maxSlotCount)
                {
                    // Add new slot with the item and initial stock (capped by maxStockPerSlot)
                    slots.Add(new ShelfSlot { item = item, stock = Mathf.Min(amount, maxStockPerSlot) });
                    return true;
                }
                else
                {
                    // No available slot for new item
                    Debug.LogWarning("No available slot for new item: " + item.DisplayName);
                    return false;
                }
            }
        }

        // Gets the specified item from the shelf if available.
        // Returns true if item was taken, false if not available.
        public bool GetItem(ItemData item, out ItemData takenItem)
        {
            takenItem = null;
            if (item == null) return false;
            var slot = slots.Find(s => s.item == item);
            if (slot != null && slot.stock > 0)
            {
                slot.stock--;
                takenItem = slot.item;
                return true;
            }
            return false;
        }

        // Gets any available item from the shelf,
        // prioritizing the first slot with stock. Returns true if an item was taken, false if shelf is empty.
        public bool GetAnyItem(out ItemData takenItem)
        {
            takenItem = null;
            // Find the first slot with stock available
            var slot = slots.Find(s => s.stock > 0);
            if (slot != null)
            {
                slot.stock--;
                takenItem = slot.item;
                return true;
            }
            return false;
        }

        public void Restock(string itemID, int amount)
        {
            var slot = slots.Find(s => s.item.ItemID == itemID);
            if (slot != null)
            {
                slot.stock = Mathf.Min(slot.stock + amount, maxStockPerSlot);
            }
        }

        public void RestockAll(int amount = 10)
        {
            foreach (var slot in slots)
            {
                slot.stock = Mathf.Min(slot.stock + amount, maxStockPerSlot);
            }
        }
    }
}
