using System;
using System.Collections.Generic;
using Project.Systems.SaveLoad;
using UnityEngine;

namespace _Project.Gameplay.Player
{
    // This class represents the player's context, which holds references to the player's controller, input, movement, state machine, inventory, and hotbar.
    // It also implements the ISaveable interface to allow saving and loading of player data.
    [Serializable]
    public class PlayerData
    {
        public InventorySaveData inventoryData;
        public HotbarSaveData hotbarData;
    }

    public class PlayerContext : ISaveable
    {
        public PlayerController Controller { get; }
        
        public IPlayerInput Input { get; }
        public IPlayerMovement Movement { get; }
        
        public PlayerStateMachine StateMachine { get; }

        public Inventory Inventory { get; }
        public Hotbar Hotbar { get; }

        public PlayerContext(PlayerController controller)
        {
            Controller = controller;
            
            Input = controller.GetComponent<IPlayerInput>();
            Movement = controller.GetComponent<IPlayerMovement>();
            
            StateMachine = new PlayerStateMachine();

            Inventory = new Inventory();
            Hotbar = new Hotbar(9);

            Inventory.OnInventoryChanged += BindInventoryHotbarChanged;
        }

        private void BindInventoryHotbarChanged(IReadOnlyList<InventorySlot> slots)
        {
            Hotbar.ValidateAllSlots();
            Hotbar.NotifyChange();
        }

        // This method saves the player's inventory and hotbar data into the provided GameData object.
        // It creates new save data objects for both inventory and hotbar,
        // populates them with the current state of the inventory and hotbar,
        // and assigns them to the PlayerData within GameData.
        public void SaveState(GameData gameData)
        {
            gameData.Player ??= new PlayerData();

            // For the inventory, we save the item ID and quantity for each slot.
            // If a slot is empty, we save null for the item ID.
            var inventorySaveData = new InventorySaveData();
            foreach (var slot in Inventory.Slots)
            {
                inventorySaveData.Slots.Add(new InventorySlotData
                {
                    ItemID = slot.IsEmpty ? null : slot.Item.ItemID,
                    Quantity = slot.Quantity
                });
            }
            gameData.Player.inventoryData = inventorySaveData;

            // For the hotbar, we save the indices of the linked inventory slots for each hotbar slot,
            // and the currently selected index.
            var hotbarSaveData = new HotbarSaveData();
            foreach(var slot in Hotbar.Slots)
            {
                if(slot.IsEmpty)
                {
                    hotbarSaveData.LinkedInventorySlotIndices.Add(-1);
                }
                else
                {
                    int index = Inventory.GetSlots().IndexOf(slot.LinkedSlot);
                    hotbarSaveData.LinkedInventorySlotIndices.Add(index);
                }
            }
            hotbarSaveData.SelectedIndex = Hotbar.SelectedIndex;
            gameData.Player.hotbarData = hotbarSaveData;
        }

        public void LoadState(GameData gameData)
        {
            var playerData = gameData.Player;
            if (playerData == null)
                return;

            // When loading the inventory, we read the saved item ID and quantity for each slot,
            var inventorySaveData = playerData.inventoryData;
            if (inventorySaveData == null)
                return;
            Inventory.ClearAll();

            var slots = Inventory.GetSlots();
            int slotCount = Mathf.Min(slots.Count, inventorySaveData.Slots.Count);
            for (int i = 0; i < slotCount; i++)
            {
                var slotData = inventorySaveData.Slots[i];
                if (string.IsNullOrEmpty(slotData.ItemID))
                    continue;

                ItemData item = ItemDictionary.Instance.GetById(slotData.ItemID);
                if (item == null)
                    continue;

                slots[i].SetItem(item, slotData.Quantity);
            }
            Inventory.NotifyChange();

            // When loading the hotbar, we need to link each hotbar slot to the corresponding inventory slot based on the saved indices.
            var hotbarSaveData = playerData.hotbarData;
            if (hotbarSaveData == null)
                return;
            Hotbar.ClearAll();

            int hotbarCount = Mathf.Min(Hotbar.SlotCount, hotbarSaveData.LinkedInventorySlotIndices.Count);
            for (int i = 0; i < hotbarCount; i++)
            {
                int linkedIndex = hotbarSaveData.LinkedInventorySlotIndices[i];

                if (linkedIndex < 0 || linkedIndex >= slots.Count)
                    continue;

                Hotbar.SetItem(i, slots[linkedIndex]);
            }
            if (hotbarSaveData.SelectedIndex >= 0 &&
                hotbarSaveData.SelectedIndex < Hotbar.SlotCount)
            {
                Hotbar.SelectSlot(hotbarSaveData.SelectedIndex);
            }
            Hotbar.NotifyChange();
        }
    }
}