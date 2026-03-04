using _Project.Gameplay.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.UI.Screens
{
    public class HotbarSlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon; // Reference to the UI Image component for displaying the item icon
        private HotbarSlot _slotData;

        public void Bind(HotbarSlot slot)
        {
            _slotData = slot;
            Refresh();
        }

        // Refresh the UI to reflect the current state of the hotbar slot
        public void Refresh()
        {
            if (_slotData != null && !_slotData.IsEmpty)
            {
                icon.sprite = _slotData.LinkedSlot.Item.Icon; // Assuming ItemData has an Icon property
                icon.enabled = true;
            }
            else
            {
                icon.sprite = null;
                icon.enabled = false;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
