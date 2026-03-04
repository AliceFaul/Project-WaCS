using UnityEngine;
using System.Collections.Generic;
using System;

namespace _Project.Gameplay.Player
{
    // This class represents the player's inventory, which can hold multiple items in different slots.
    [Serializable]
    public class InventorySaveData
    {
        public List<InventorySlotData> Slots = new();
    }

    // This class represents the data for a single inventory slot, which can be saved and loaded.
    [Serializable]
    public class InventorySlotData
    {
        public string ItemID;
        public int Quantity;
    }

    [System.Serializable]
    public class InventorySlot
    {
        public ItemData Item { get; private set; }
        public int Quantity { get; private set; }

        public bool IsEmpty => Item == null || Quantity <= 0;

        public void SetItem(ItemData item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public void AddQuantity(int amount = 1)
        {
            Quantity += amount;
        }

        public void RemoveQuantity(int amount = 1)
        {
            Quantity -= amount;
            if (Quantity <= 0)
            {
                Clear();
            }
        }

        public void Clear()
        {
            Item = null;
            Quantity = 0;
        }
    }

    [System.Serializable]
    public class Inventory
    {
        private readonly List<InventorySlot> _slots;

        public IReadOnlyList<InventorySlot> Slots => _slots;
        public List<InventorySlot> GetSlots() => _slots; // For saving/loading purposes, we need a mutable list
        public int SlotCount => _slots.Count;

        public event Action<IReadOnlyList<InventorySlot>> OnInventoryChanged;

        public Inventory(int slotCount = 25)
        {
            _slots = new List<InventorySlot>(slotCount);
            for (int i = 0; i < slotCount; i++)
            {
                _slots.Add(new InventorySlot());
            }
        }

        // INVENTORY MANAGEMENT

        // Returns true if all items were added, false if some couldn't be added due to lack of space
        public bool AddItem(ItemData item, int quantity = 1)
        {

            if (item == null || quantity <= 0)
            {
                Debug.LogWarning("Invalid item or quantity");
                return false;
            }
            int remaining = quantity;

            // Try to stack in existing slots
            foreach (var slot in _slots)
            {
                if (slot.IsEmpty)
                    continue;
                if (slot.Item.ItemID == item.ItemID &&
                    slot.Quantity < item.MaxStackSize)
                {
                    int space = item.MaxStackSize - slot.Quantity;
                    int addAmount = Mathf.Min(space, remaining);
                    slot.AddQuantity(addAmount);
                    remaining -= addAmount;
                    if (remaining <= 0)
                    {
                        OnInventoryChanged?.Invoke(_slots);
                        return true;
                    }
                }
            }

            // Try to add to empty slots
            foreach (var slot in _slots)
            {
                if (!slot.IsEmpty)
                    continue;
                int addAmount = Mathf.Min(item.MaxStackSize, remaining);
                slot.SetItem(item, addAmount);
                remaining -= addAmount;
                if (remaining <= 0)
                {
                    OnInventoryChanged?.Invoke(_slots);
                    return true;
                }
            }

            // Inventory full, some items couldn't be added
            OnInventoryChanged?.Invoke(_slots);
            return false;
        }

        public bool RemoveItem(ItemData item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
            {
                Debug.LogWarning("Invalid item or quantity");
                return false;
            }
            int remaining = quantity;
            foreach (var slot in _slots)
            {
                if (slot.IsEmpty) continue;
                if (slot.Item.ItemID != item.ItemID) continue;

                int removeAmount = Mathf.Min(slot.Quantity, remaining);
                slot.RemoveQuantity(removeAmount);
                remaining -= removeAmount;
                if (remaining <= 0)
                {
                    OnInventoryChanged?.Invoke(_slots);
                    return true;
                }
            }
            // Not enough items to remove
            OnInventoryChanged?.Invoke(_slots);
            return false;
        }

        // Returns true if the inventory contains at least one of the specified item
        public bool HasItem(string itemID)
        {
            if (itemID == null) return false;
            foreach (var slot in _slots)
            {
                if (slot.IsEmpty) continue;
                if (slot.Item.ItemID == itemID)
                {
                    return true;
                }
            }
            return false;
        }

        // Helper method to find the first slot containing the specified item, or null if not found
        public void NotifyChange()
        {
            OnInventoryChanged?.Invoke(_slots);
        }

        public void ClearAll()
        {
            foreach (var slot in _slots)
            {
                slot.Clear();
            }
            NotifyChange();
        }
    }
}
