using UnityEngine;
using UnityEngine.UI;
using _Project.UI.Screens;

namespace _Project.Systems.Game
{
    public class ItemDragHandler : MonoBehaviour
    {
        public static ItemDragHandler Instance { get; private set; }

        [SerializeField] private Image dragIcon;
        [SerializeField] private CanvasGroup dragCanvasGroup;

        private InventorySlotUI inventorySlot;
        private ItemData draggedItem;

        private bool isDragging;

        private void Awake()
        {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);

            dragIcon.enabled = false;
            dragCanvasGroup.blocksRaycasts = false;
        }

        public void BeginDrag(InventorySlotUI slot)
        {

        }
    }
}
