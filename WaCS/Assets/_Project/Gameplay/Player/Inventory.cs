using UnityEngine;
using System.Collections.Generic;
using System;

namespace _Project.Gameplay.Player
{
    [System.Serializable]
    public class InventorySlot
    {
        public ItemData Item { get; private set; }
        public int Quantity { get; private set; }

        public bool IsEmpty => Item == null;

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
        public int SlotCount => _slots.Count;

        public event Action<IReadOnlyList<InventorySlot>> OnInventoryChanged;

        public Inventory(int slotCount = 25)
        {
            _slots = new List<InventorySlot>(slotCount);
            for(int i = 0; i < slotCount; i++)
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
    }
}
