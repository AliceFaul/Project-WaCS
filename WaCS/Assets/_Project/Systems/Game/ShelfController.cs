using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Systems.SaveLoad;

namespace _Project.Systems.Game
{
    public class ShelfController : MonoBehaviour
    {
        [SerializeField] private List<ShelfPoint> shelfPoints;
        [SerializeField] private string shelfID;

        public string ShelfID => shelfID;

        private void Awake()
        {
            shelfID = string.IsNullOrEmpty(shelfID) ? System.Guid.NewGuid().ToString() : gameObject.name;
            // If no shelf points are assigned in the inspector, automatically populate the list with child ShelfPoint components
            if (shelfPoints == null || shelfPoints.Count == 0)
            {
                shelfPoints = GetComponentsInChildren<ShelfPoint>().ToList();
            }
        }

        private async void Start()
        {
            await Task.Yield();
            if(!ServiceRegistry.TryGet<ShelfService>(out var shelfService))
            {
                await Task.Yield();
            }
            shelfService?.RegisterShelf(this);
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

        public void LoadFromData(ShelfSaveData data)
        {
            var points = GetAllShelfPoints().ToList();
            for(int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                var pointData = data.shelfPoints[i];
                
                while(!point.IsEmpty)
                {
                    point.RemoveOne();
                }

                if(string.IsNullOrEmpty(pointData.itemID))
                    continue;

                var item = ItemDictionary.Instance.GetById(pointData.itemID);
                point.Init(item);
                point.SetPrice(pointData.price);
                for(int q = 0; q < pointData.quantity; q++)
                {
                    point.AddOne();
                }
            }
        }

        public IEnumerable<ShelfPoint> GetAllShelfPoints()
        {
            return shelfPoints;
        }
    }
}
