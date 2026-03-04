using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace _Project.Systems.Game
{
    public class ShelfController : MonoBehaviour
    {
        [SerializeField] private List<ShelfPoint> shelfPoints;

        private void Awake()
        {
            // If no shelf points are assigned in the inspector, automatically populate the list with child ShelfPoint components
            if (shelfPoints == null || shelfPoints.Count == 0)
            {
                shelfPoints = GetComponentsInChildren<ShelfPoint>().ToList();
            }
        }

        // Tries to place an item on the shelf.
        // Returns true if successful, false if the shelf is full or item is null.
        public bool TryPlaceItem(ItemData item)
        {
            if (item == null)
            {
                Debug.LogWarning("Cannot place null item on shelf");
                return false;
            }

            // First try to find a shelf point that already has the same item type and can accept more items
            foreach (var point in shelfPoints)
            {
                if (point.HasItems && point.CanAdd(item))
                {
                    point.AddOne();
                    return true;
                }
            }

            // If no existing stack can accept the item, try to find an empty shelf point
            foreach (var point in shelfPoints)
            {
                if (point.IsEmpty)
                {
                    point.Init(item);
                    point.AddOne();
                    return true;
                }
            }
            // If no shelf point can accept the item, return false
            return false;
        }

        public IEnumerable<ShelfPoint> GetAllShelfPoints()
        {
            return shelfPoints;
        }
    }
}
