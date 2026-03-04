using UnityEngine;
using System.Collections.Generic;

namespace _Project.Systems.Game
{
    public class ShelfPoint : MonoBehaviour
    {
        [SerializeField] private int capacity = 12;
        [SerializeField] private float spacingX = 0.2f;
        [SerializeField] private float spacingZ = 0.25f;
        [SerializeField] private float heightOffset = 0.02f;
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
        public bool HasItems => quantity > 0;

        public Transform CustomerStandPoint => customerStandPoint;

        public void Init(ItemData itemType)
        {
            this.itemType = itemType;
            price = itemType.SellPrice;
        }

        // Tries to add an item to the shelf point.
        public bool TryAdd(ItemData item)
        {
            if(item == null)
            {
                Debug.LogWarning("Cannot add null item to shelf point");
                return false;
            }
            if(IsEmpty)
            {
                Init(item);
            }
            if(itemType.ItemID != item.ItemID)
            {
                Debug.LogWarning("Cannot add item to shelf point: item type does not match");
                return false;
            }
            if (IsFull)
            {
                Debug.LogWarning("Cannot add item to shelf point: shelf is full");
                return false;
            }
            AddOne();
            return true;
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

                Vector3 offset = new Vector3(
                    col * spacingX,
                    heightOffset,
                    -row * spacingZ
                );

                Transform item = _spawnedItems[i].transform;
                item.localPosition = offset;

                item.rotation = Quaternion.LookRotation(-itemHolder.forward);
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
