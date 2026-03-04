using _Project.Gameplay.Player;
using _Project.Systems.Game;
using _Project.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.UI.Screens
{
    public class InventorySlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon; // Reference to the UI Image component for displaying the item icon
        [SerializeField] private TMP_Text quantityText; // Reference to the TextMeshPro component for displaying item quantity
        private InventorySlot _slotData;

        public InventorySlot GetData() => _slotData;

        public void Bind(InventorySlot slot)
        {
            _slotData = slot;
            Refresh();
        }

        public void Refresh()
        {
            if (_slotData != null && !_slotData.IsEmpty)
            {
                icon.sprite = _slotData.Item.Icon; // Assuming ItemData has an Icon property
                icon.enabled = true;
                quantityText.text = _slotData.Quantity > 1 ? _slotData.Quantity.ToString() : "";
            }
            else
            {
                icon.sprite = null;
                icon.enabled = false;
                quantityText.text = "";
            }
        }

        // Called when the user clicks on the inventory slot
        public void OnPointerDown(PointerEventData eventData)
        {
            ItemDragHandler.Instance.BeginDrag(this);
        }

        // Called when the user's pointer enters the inventory slot (for hover effects)
        public void OnPointerEnter(PointerEventData eventData)
        {
            RectTransform rect = GetComponent<RectTransform>();
            TweenHelper.Instance.HoverEnter(rect);
        }

        // Called when the user's pointer exits the inventory slot (for hover effects)
        public void OnPointerExit(PointerEventData eventData)
        {
            RectTransform rect = GetComponent<RectTransform>();
            TweenHelper.Instance.HoverExit(rect);
        }
    }
}
