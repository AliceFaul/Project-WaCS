using UnityEngine;
using System.Collections.Generic;

namespace _Project.Systems.Game
{
    public class ShelfPoint : MonoBehaviour
    {
        [SerializeField] private int capacity = 12;
        [SerializeField] private List<Transform> anchors = new();
        [SerializeField] private Transform customerStandPoint;

        [SerializeField] private ItemData itemType;
        [SerializeField] private int quantity;
        [SerializeField] private float price;

        private readonly List<GameObject> _spawnedItems = new();

        public ItemData ItemType => itemType;
        public int Quantity => quantity;
        public float Price => price;

        public bool IsEmpty => quantity <= 0;
        public bool IsFull => quantity >= capacity;
        public bool HasItems => itemType != null;

        public Transform CustomerStandPoint => customerStandPoint;

        private void Awake()
        {
            // If no anchors are assigned in the inspector,
            // automatically populate the list with child transforms
            if (anchors == null || anchors.Count == 0)
            {
                foreach (Transform child in transform)
                {
                    anchors.Add(child);
                }
            }
        }

        public void Init(ItemData itemType)
        {
            this.itemType = itemType;
            price = itemType.SellPrice;
        }

        // Adds an item to the shelf point.
        // Returns true if successful, false if the shelf is full or item type doesn't match.
        public bool CanAdd(ItemData item)
        {
            if (IsFull)
                return false;
            if (IsEmpty)
                return true;
            return itemType.ItemID == item.ItemID;
        }

        public void AddOne()
        {
            if(itemType == null)
            {
                Debug.LogError("Cannot add item to shelf point: item type is null");
                return;
            }
            if(IsFull)
            {
                Debug.LogWarning("Cannot add item to shelf point: shelf is full");
                return;
            }
            if(quantity >= anchors.Count)
            {
                Debug.LogWarning("Not enough anchors to display all items on the shelf");
                return;
            }

            Transform anchor = anchors[quantity];
            GameObject itemObj = 
                Instantiate(itemType.Prefab, anchor.position, anchor.rotation, transform);
            _spawnedItems.Add(itemObj);
            quantity++;
        }

        // Removes one item from the shelf point.
        public void RemoveOne()
        {
            if(IsEmpty)
            {
                Debug.LogWarning("Cannot remove item from shelf point: shelf is empty");
                return;
            }
            quantity--;
            GameObject last = _spawnedItems[quantity];
            _spawnedItems.RemoveAt(quantity);
            Destroy(last);

            if(quantity == 0)
            {
                itemType = null;
                price = 0f;
            }
        }

        public void SetPrice(float newPrice)
        {
            price = Mathf.Max(0f, newPrice);
        }
    }
}
