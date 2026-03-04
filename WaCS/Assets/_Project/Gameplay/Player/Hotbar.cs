using UnityEngine;
using System;
using System.Collections.Generic;

namespace _Project.Gameplay.Player
{
    public class EquipmentItem
    {
        public string Name { get; }
        public Sprite Icon { get; }
        public Action OnUse { get; }
        public EquipmentItem(string name, Sprite icon, Action onUse)
        {
            Name = name;
            Icon = icon;
            OnUse = onUse;
        }
    }

    public class HotbarSlot
    {
        public EquipmentItem Item { get; private set; }
        public bool IsEmpty => Item == null;
        public void SetItem(EquipmentItem item)
        {
            Item = item;
        }
        public void Clear()
        {
            Item = null;
        }
    }

    [System.Serializable]
    public class Hotbar
    {
        private readonly List<HotbarSlot> _slots = new List<HotbarSlot>();
        private int _selectedIndex = 0;

        public int SlotCount => _slots.Count;
        public int SelectedIndex => _selectedIndex;
        public HotbarSlot SelectedSlot => _slots.Count > 0 ? _slots[_selectedIndex] : null;

        public event Action<int, EquipmentItem> OnHotbarChanged;
        public event Action<int> OnSlotSelected;

        public Hotbar(int slotCount)
        {
            _slots = new List<HotbarSlot>(slotCount);
            for (int i = 0; i < slotCount; i++)
            {
                _slots.Add(new HotbarSlot());
            }
        }

        public void SetItem(int slot, EquipmentItem item)
        {
            if (slot < 0 || slot >= _slots.Count)
            {
                Debug.LogWarning("Invalid hotbar slot index.");
                return;
            }
            _slots[slot].SetItem(item);
            OnHotbarChanged?.Invoke(slot, item);
        }

        public void ClearSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Count)
            {
                Debug.LogWarning("Invalid hotbar slot index.");
                return;
            }
            _slots[slotIndex].Clear();
            OnHotbarChanged?.Invoke(slotIndex, null);
        }

        public void SelectSlot(int slotIndex)
        {
            if(slotIndex > _slots.Count)
            {
                Debug.LogWarning("Invalid hotbar slot index.");
                return;
            }
            _selectedIndex = slotIndex;
            OnSlotSelected?.Invoke(slotIndex);
        }

        public void UseSelectedItem()
        {
            var selectedSlot = SelectedSlot;
            if (selectedSlot != null && !selectedSlot.IsEmpty)
            {
                selectedSlot.Item.OnUse?.Invoke();
            }
        }
    }
}
