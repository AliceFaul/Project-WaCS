using _Project.Gameplay.Player;
using UnityEngine;
using System.Collections.Generic;
using Project.Systems.Game;

namespace _Project.UI.Screens
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Transform inventoryContent;
        [SerializeField] private GameObject slotPrefab;

        private Inventory _inventory;
        private List<GameObject> _slotUIs = new List<GameObject>();

        public void Init(Inventory inventory)
        {
            _inventory = inventory;
            transform.gameObject.SetActive(false);
            CreateSlots();
            UpdateUI(_inventory.Slots);
            _inventory.OnInventoryChanged += UpdateUI;
        }

        public void BindInput(PlayerController controller)
        {
            controller.OnInventoryToggled += ToggleInventory;
        }

        private void OnDestroy()
        {
            if(_inventory != null)
                _inventory.OnInventoryChanged -= UpdateUI;
        }

        public void ToggleInventory(bool isOpen)
        {
            transform.gameObject.SetActive(isOpen);
            PauseSystem.PauseGame(isOpen);
            Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;
        }

        // Creates UI slots based on the inventory's slot count
        private void CreateSlots()
        {
            // Clear existing slots
            foreach(Transform child in inventoryContent)
            {
                Destroy(child.gameObject);
            }
            _slotUIs.Clear();
            // Create UI slots based on inventory slot count
            for(int i = 0; i < _inventory.SlotCount; i++)
            {
                var slot = Instantiate(slotPrefab, inventoryContent);
                var slotUI = slot.GetComponent<InventorySlotUI>();
                slotUI.Bind(_inventory.Slots[i]);
                _slotUIs.Add(slot);
            }
        }

        private void UpdateUI(IReadOnlyList<InventorySlot> slots)
        {
            for(int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                var slotUI = _slotUIs[i];
                slotUI.GetComponent<InventorySlotUI>().Refresh();
            }
        }
    }
}
