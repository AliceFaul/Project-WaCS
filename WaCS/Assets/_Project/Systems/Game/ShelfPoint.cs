using UnityEngine;
using System.Collections.Generic;

namespace _Project.Systems.Game
{
    public class ShelfPoint : MonoBehaviour
    {
        [SerializeField] private int capacity = 12;
        [SerializeField] private float spacingX = 0.5f;
        [SerializeField] private float spacingZ = 0.5f;
        [SerializeField] private float itemPerRow = 6;
        [SerializeField] private Transform itemHolder;
        [SerializeField] private Transform customerStandPoint;

        private ItemData itemType;
        private int quantity;
        private float price;

        private readonly List<GameObject> _spawnedItems = new();

        // Properties to access the shelf point's state
        public ItemData ItemType => itemType;
        public int Quantity => quantity;
        public float Price => price;

        public bool IsEmpty => quantity <= 0;
        public bool IsFull => quantity >= capacity;
        public bool HasItems => itemType != null;

        public Transform CustomerStandPoint => customerStandPoint;

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

            GameObject itemObj = 
                Instantiate(itemType.Prefab, itemHolder);
            _spawnedItems.Add(itemObj);
            AutoArranged();
            quantity++;
        }

        private void AutoArranged()
        {
            for (int i = 0; i < _spawnedItems.Count; i++)
            {
                int row = i / (int)itemPerRow;
                int col = i % (int)itemPerRow;
                Vector3 offset = new Vector3(col * spacingX, 0f, row * spacingZ);
                _spawnedItems[i].transform.localPosition = offset;
            }
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
