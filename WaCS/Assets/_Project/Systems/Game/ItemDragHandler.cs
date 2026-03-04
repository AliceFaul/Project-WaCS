using UnityEngine;
using UnityEngine.UI;
using _Project.UI.Screens;
using _Project.Gameplay.Player;
using _Project.UI.Animations;
using UnityEngine.EventSystems;

namespace _Project.Systems.Game
{
    public class ItemDragHandler : MonoBehaviour
    {
        public static ItemDragHandler Instance { get; private set; }

        [SerializeField] private Image dragIcon;
        [SerializeField] private CanvasGroup dragCanvasGroup;
        [SerializeField] private Canvas canvas;

        private InventorySlotUI _sourceSlotUI;
        private InventorySlot _draggedItem;

        private RectTransform _dragRect;
        private bool _isDragging;

        private void Awake()
        {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);

            dragIcon.gameObject.SetActive(false);
            dragCanvasGroup.interactable = false; // Prevent interactions with the drag icon itself
            dragCanvasGroup.blocksRaycasts = false;
            _dragRect = dragIcon.GetComponent<RectTransform>();
        }

        // Called when the user starts dragging an item from an inventory slot
        public void BeginDrag(InventorySlotUI slot, PointerEventData eventData)
        {
            if(slot.GetData().IsEmpty)
            {
                Debug.LogWarning("Attempted to drag an empty slot.");
                return;
            }
            _sourceSlotUI = slot;
            _draggedItem = slot.GetData();
            _isDragging = true;

            dragIcon.sprite = _draggedItem.Item.Icon; // Assuming ItemData has an Icon property
            dragIcon.gameObject.SetActive(true);
            dragCanvasGroup.blocksRaycasts = false; // Block raycasts to prevent UI interactions while dragging

            dragIcon.transform.SetParent(canvas.transform);
            dragIcon.transform.SetAsLastSibling(); // Ensure the drag icon is on top of other UI elements
            UpdateDragPosition(eventData);
        }

        public void EndDrag()
        {
            _isDragging = false;
            dragIcon.gameObject.SetActive(false);
            dragCanvasGroup.blocksRaycasts = true; // Allow raycasts again for UI interactions
            dragIcon.transform.SetParent(transform); // Reset parent to avoid cluttering the canvas hierarchy
            _sourceSlotUI = null;
            _draggedItem = null;
        }

        // Called when the user drags the item (updates the position of the drag icon)
        public void UpdateDragPosition(PointerEventData eventData)
        {
            if(!_isDragging)
                return;
            // Convert the screen point to a local point in the canvas and update the drag icon's position
            RectTransformUtility.
                ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, 
                eventData.position, 
                eventData.pressEventCamera, 
                out Vector2 localPoint);
            _dragRect.localPosition = localPoint;
        }

        // Called when the user drops an item onto a hotbar slot
        public void DropOnHotbar(HotbarSlotUI slotUI)
        {
            if(!_isDragging || _draggedItem == null)
            {
                Debug.LogWarning("No item is being dragged.");
                return;
            }
            // Link the dragged item to the hotbar slot and refresh the UI
            HotbarSlot hotbarSlot = slotUI.GetData();
            var hotbarUI = slotUI.GetComponentInParent<HotbarUI>();
            foreach (var slot in hotbarUI.GetHotbar().Slots)
            {
                if (slot.LinkedSlot == _draggedItem)
                {
                    slot.Clear(); // Clear the previous hotbar slot that linked to this item
                    break;
                }
            }
            hotbarSlot.SetLink(_draggedItem);
            hotbarUI.RefreshAll();
            EndDrag();
        }

        // Called when the user drops an item back into the inventory (or anywhere else that accepts drops)
        public InventorySlot GetDraggedItem() => _draggedItem;
    }
}
