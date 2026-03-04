using _Project.Gameplay.Player;
using _Project.Systems.Game;
using _Project.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.UI.Screens
{
    public class HotbarSlotUI : MonoBehaviour, 
        IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon; // Reference to the UI Image component for displaying the item icon
        private HotbarSlot _slotData;

        public HotbarSlot GetData() => _slotData;

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
                icon.gameObject.SetActive(true);
            }
            else
            {
                icon.sprite = null;
                icon.gameObject.SetActive(false);
            }
        }

        // Called when an item is dropped onto this hotbar slot
        public void OnDrop(PointerEventData eventData)
        {
            if(ItemDragHandler.Instance.GetDraggedItem() == null)
            {
                Debug.LogError("No item being dragged.");
                return;
            }
            Debug.Log("Drop triggered hotbar"); // Debug log to confirm the drop event is being detected
            ItemDragHandler.Instance.DropOnHotbar(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            RectTransform rect = GetComponent<RectTransform>();
            TweenHelper.Instance.HoverEnter(rect);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            RectTransform rect = GetComponent<RectTransform>();
            TweenHelper.Instance.HoverExit(rect);
        }
    }
}
