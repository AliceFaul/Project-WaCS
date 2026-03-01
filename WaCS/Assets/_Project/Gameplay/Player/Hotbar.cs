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
        private readonly List<HotbarSlot> _slots;
        private int _selectedIndex = 0;

        public int SlotCount => _slots.Count;
        public int SelectedIndex => _selectedIndex;
        public HotbarSlot SelectedSlot 
            => _slots.Count > 0 ? _slots[_selectedIndex] : null;

        public IReadOnlyList<HotbarSlot> Slots => _slots;

        public event Action<IReadOnlyList<HotbarSlot>> OnHotbarChanged;
        public event Action<int> OnSlotSelected;

        public Hotbar(int slotCount)
        {
            _slots = new List<HotbarSlot>(slotCount);
            for(int i = 0; i < slotCount; i++)
            {
                _slots.Add(new HotbarSlot());
            }
        }

        // SLOT MANAGEMENT
        public void SetItem(int slotIndex, EquipmentItem item)
        {
            if(!IsValidIndex(slotIndex))
            {
                Debug.LogWarning("Invalid hotbar slot index");
                return;
            }
            _slots[slotIndex].SetItem(item);
            OnHotbarChanged?.Invoke(_slots);
        }

        public void ClearSlot(int slotIndex)
        {
            if (!IsValidIndex(slotIndex))
            {
                Debug.LogWarning("Invalid hotbar slot index");
                return;
            }
            _slots[slotIndex].Clear();
            OnHotbarChanged?.Invoke(_slots);
        }

        // SELECTION
        public void SelectSlot(int slotIndex)
        {
            if(!IsValidIndex(slotIndex))
            {
                Debug.LogWarning("Invalid hotbar slot index");
                return;
            }
            if(_selectedIndex == slotIndex)
            {
                Debug.Log("Slot already selected");
                return;
            }
            _selectedIndex = slotIndex;
            OnSlotSelected?.Invoke(_selectedIndex);
        }

        public void SelectNext()
        {
            if (_slots.Count == 0) return;
            int next = (_selectedIndex + 1) % _slots.Count;
            SelectSlot(next);
        }

        public void SelectPrevious()
        {
            if (_slots.Count == 0) return;
            int prev = (_selectedIndex - 1 + _slots.Count) % _slots.Count;
            SelectSlot(prev);
        }

        // BIND INPUT
        public void BindInput(IPlayerInput input)
        {
            if(input == null) return;

            if(input.HotbarNext)
                SelectNext();
            
            if(input.HotbarPrevious)
                SelectPrevious();
            
            if(input.HotbarNumberPressed >= 0)
                SelectSlot(input.HotbarNumberPressed);
        }

        // USE ITEM
        public void UseSelectedItem()
        {
            var slot = SelectedSlot;
            if(slot == null || slot.IsEmpty)
            {
                Debug.LogWarning("No item in selected hotbar slot");
                return;
            }
            slot.Item.OnUse?.Invoke();
        }

        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < _slots.Count;
        }
    }
}
