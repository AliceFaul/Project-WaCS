using UnityEngine;
using System;
using System.Collections.Generic;

namespace _Project.Gameplay.Player
{
    public class HotbarSlot
    {
        public InventorySlot LinkedSlot { get; private set; }
        public bool IsEmpty => LinkedSlot == null || LinkedSlot.IsEmpty;
        
        public void SetItem(InventorySlot item)
        {
            LinkedSlot = item;
        }

        public void SetLink(InventorySlot inventorySlot)
        {
            LinkedSlot = inventorySlot;
        }

        public void Clear()
        {
            LinkedSlot = null;
        }
    }

    [System.Serializable]
    public class Hotbar
    {
        private readonly List<HotbarSlot> _slots;
        private int _selectedIndex = -1;

        public int SlotCount => _slots.Count;
        public int SelectedIndex => _selectedIndex;
        public HotbarSlot SelectedSlot 
            => IsValidIndex(_selectedIndex) ? _slots[_selectedIndex] : null;

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
        public void SetItem(int slotIndex, InventorySlot item)
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
            if(_selectedIndex == slotIndex)
            {
                Deselect();
            }
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
                Deselect();
                return;
            }
            if (_slots[slotIndex].IsEmpty)
            {
                Deselect();
                Debug.Log("Selected empty slot, deselecting");
                return;
            }
            _selectedIndex = slotIndex;
            OnSlotSelected?.Invoke(_selectedIndex);
            Debug.Log($"Selected hotbar slot: {_selectedIndex}");
        }

        public void Deselect()
        {
            _selectedIndex = -1;
            OnSlotSelected?.Invoke(_selectedIndex);
            Debug.Log("Deselected hotbar slot");
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

        // HELPERS
        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < _slots.Count;
        }

        public ItemData GetEquippedItem()
        {
            var slot = SelectedSlot;
            return slot != null && !slot.IsEmpty ? slot.LinkedSlot.Item : null;
        }

        public bool HasEquippedItem()
        {
            var slot = SelectedSlot;
            return slot != null && !slot.IsEmpty;
        }
    }
}
