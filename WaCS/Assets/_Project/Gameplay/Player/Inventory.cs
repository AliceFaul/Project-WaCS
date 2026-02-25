using UnityEngine;
using System.Collections.Generic;
using System;

namespace _Project.Gameplay.Player
{
    [System.Serializable]
    public class InventorySlot
    {
        public ItemData item;
        public int quantity;

        public bool IsEmpty => item == null || quantity <= 0;

        public void Clear()
        {
            item = null;
            quantity = 0;
        }
    }

    [System.Serializable]
    public class Inventory
    {
        public List<InventorySlot> slots = new List<InventorySlot>();
        public int maxSlotCount = 20;

        public event Action<List<InventorySlot>> OnInventoryChanged;

        public Inventory(int slotCount = 20)
        {
            maxSlotCount = slotCount;
        }

        // Returns true if item was added successfully, false if inventory full or invalid input
        public bool AddItem(ItemData item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
            {
                Debug.LogWarning("Invalid item or quantity. Cannot add to inventory.");
                return false;
            }
            // Try to find an existing slot for this item
            var slot = slots.Find(s => s.item != null && s.item.ItemID == item.ItemID);
            if (slot != null)
            {
                // If found, increase quantity
                slot.quantity += quantity;
                OnInventoryChanged?.Invoke(slots);
                return true;
            }
            else
            {
                // If not found, try to add a new slot if we have capacity
                if (slots.Count < maxSlotCount)
                {
                    slots.Add(new InventorySlot { item = item, quantity = quantity });
                    OnInventoryChanged?.Invoke(slots);
                    return true;
                }
            }
            // Inventory full or invalid input
            return false;
        }

        // Returns true if item was removed successfully,
        // false if item not found or not enough quantity
        public bool RemoveItem(ItemData item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
            {
                Debug.LogWarning("Invalid item ID or quantity. Cannot remove from inventory.");
                return false;
            }
            var slot = slots.Find(s => s.item != null && s.item.ItemID == item.ItemID);
            if (slot != null && slot.quantity >= quantity)
            {
                slot.quantity -= quantity;
                if (slot.quantity <= 0)
                {
                    slot.Clear();
                }
                OnInventoryChanged?.Invoke(slots);
                return true;
            }
            // Item not found or not enough quantity
            return false;
        }
    }
}
