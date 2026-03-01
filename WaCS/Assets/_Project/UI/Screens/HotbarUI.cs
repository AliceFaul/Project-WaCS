using _Project.Gameplay.Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace _Project.UI.Screens
{
    public class HotbarUI : MonoBehaviour
    {
        [SerializeField] private Transform hotbarContent;
        [SerializeField] private GameObject slotPrefab;

        private Hotbar _hotbar;
        private List<GameObject> _slotUIs = new List<GameObject>();

        public void Init(Hotbar hotbar)
        {
            _hotbar = hotbar;

            CreateSlots();
            UpdateUI(_hotbar.Slots);
            HighlightSlot(_hotbar.SelectedIndex);
            
            _hotbar.OnHotbarChanged += UpdateUI;
            _hotbar.OnSlotSelected += HighlightSlot;
        }

        private void OnDestroy()
        {
            if (_hotbar != null)
            {
                _hotbar.OnHotbarChanged -= UpdateUI;
                _hotbar.OnSlotSelected -= HighlightSlot;
            }
        }

        private void CreateSlots()
        {
            foreach(Transform child in hotbarContent)
            {
                Destroy(child.gameObject);
            }
            _slotUIs.Clear();
            for(int i = 0; i < _hotbar.SlotCount; i++)
            {
                var slot = Instantiate(slotPrefab, hotbarContent);
                _slotUIs.Add(slot);
                
                var numberText = slot.transform.Find("Number").GetComponent<TMP_Text>();
                numberText.text = (i + 1).ToString();
            }
        }

        private void UpdateUI(IReadOnlyList<HotbarSlot> slots)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                var slotUI = _slotUIs[i];
                var itemIcon = slotUI.transform.Find("Icon").GetComponent<Image>();

                if (slot.IsEmpty)
                {
                    itemIcon.sprite = null;
                    itemIcon.enabled = false;
                }
                else
                {
                    itemIcon.sprite = slot.Item.Icon;
                    itemIcon.enabled = true;
                }
            }
        }

        private void HighlightSlot(int index)
        {
            for(int i = 0; i < _slotUIs.Count; i++)
            {
                var highlight = _slotUIs[i].
                    transform.Find("Highlight").GetComponent<Image>();
                highlight.enabled = (i == index);
                highlight.color = (i == index) ? Color.yellow : new Color(1, 1, 0, 0.5f);
            }
        }
    }
}
