using _Project.Gameplay.Player;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

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
            CreateSlots();
            UpdateUI(_inventory.Slots);
            _inventory.OnInventoryChanged += UpdateUI;
        }

        private void OnDestroy()
        {
            if(_inventory != null)
                _inventory.OnInventoryChanged -= UpdateUI;
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
                _slotUIs.Add(slot);
            }
        }

        private void UpdateUI(IReadOnlyList<InventorySlot> slots)
        {
            for(int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                var slotUI = _slotUIs[i];
                var itemIcon = slotUI.transform.Find("Icon").GetComponent<Image>();
                var quantityText = slotUI.transform.Find("Quantity").GetComponent<TMP_Text>();

                if(slot.IsEmpty)
                {
                    itemIcon.sprite = null;
                    itemIcon.enabled = false;
                    quantityText.text = "";
                }
                else
                {
                    itemIcon.sprite = slot.Item.Icon;
                    itemIcon.enabled = true;
                    quantityText.text = slot.Quantity > 1 ? slot.Quantity.ToString() : "";
                }
            }
        }
    }
}
