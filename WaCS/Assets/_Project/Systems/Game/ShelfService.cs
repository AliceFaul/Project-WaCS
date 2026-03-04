using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using Project.Systems.SaveLoad;
using System.Linq;

namespace _Project.Systems.Game
{
    [System.Serializable]
    public class ShelfPointData
    {
        public string itemID;
        public int quantity;
        public float price;
    }

    // Data class for saving and loading shelf state.
    // It contains the shelf's unique ID and a list of ShelfPointData representing the state of each shelf point on the shelf.
    [System.Serializable]
    public class ShelfSaveData
    {
        public string shelfID;
        public List<ShelfPointData> shelfPoints;
    }

    // Service responsible for managing all shelves in the game.
    // It keeps track of all ShelfController instances and provides methods to register and unregister shelves.
    public class ShelfService : IManager, ISaveable
    {
        private readonly List<ShelfController> _shelves = new();

        public async Task<bool> InitAsync()
        {
            await Task.CompletedTask;
            return true;
        }

        // Finds all ShelfController instances in the scene and registers them with the service.
        public void RegisterAllShelves()
        {
            _shelves.Clear();
            var shelves = Object.FindObjectsByType<ShelfController>(FindObjectsSortMode.None);
            foreach (var shelf in shelves)
            {
                RegisterShelf(shelf);
            }
            Debug.Log($"Registered {_shelves.Count} shelves in the scene");
        }

        // Registers a single ShelfController instance with the service.
        // This method is called by ShelfController instances when they are initialized.
        public void RegisterShelf(ShelfController shelf)
        {
            if (shelf == null)
            {
                Debug.LogWarning("Cannot register null shelf");
                return;
            }
            if (!_shelves.Contains(shelf))
                _shelves.Add(shelf);
            Debug.Log($"Registered shelf {shelf.ShelfID}");
        }

        // Unregisters a ShelfController instance from the service.
        public void UnregisterShelf(ShelfController shelf)
        {
            if (shelf == null)
            {
                Debug.LogWarning("Cannot unregister null shelf");
                return;
            }
            _shelves.Remove(shelf);
        }

        // Returns a random ShelfController from the list of registered shelves,
        // or null if no shelves are registered.
        public ShelfPoint GetRandomAvailablePoint()
        {
            var points = _shelves
                .SelectMany(s => s.GetAllShelfPoints())
                .Where(p => p.HasItems)
                .ToList();

            if (points.Count == 0)
                return null;

            return points[Random.Range(0, points.Count)];
        }

        // Searches through all registered shelves and
        // their shelf points to find a point that has items matching the specified item ID.
        public ShelfPoint GetPointByItem(string itemId)
        {
            foreach (var shelf in _shelves)
            {
                foreach (var point in shelf.GetAllShelfPoints())
                {
                    if (point.HasItems &&
                        point.ItemType.ItemID == itemId)
                        return point;
                }
            }

            return null;
        }

        // Saves the state of all registered shelves to the provided GameData object.
        public void SaveState(GameData gameData)
        {
            gameData.Shelves.Clear();
            foreach (var shelf in _shelves)
            {
                var shelfData = new ShelfSaveData
                {
                    shelfID = shelf.ShelfID,
                    shelfPoints = new List<ShelfPointData>()
                };
                foreach (var point in shelf.GetAllShelfPoints())
                {
                    var pointData = new ShelfPointData
                    {
                        itemID = point.HasItems ? point.ItemType.ItemID : string.Empty,
                        quantity = point.Quantity,
                        price = point.Price
                    };
                    shelfData.shelfPoints.Add(pointData);
                }
                gameData.Shelves.Add(shelfData);
            }
        }

        // Loads the state of all registered shelves from the provided GameData object.
        public void LoadState(GameData gameData)
        {
            foreach (var shelf in _shelves)
            {
                var saved = gameData.Shelves.
                    FirstOrDefault(s => s.shelfID == shelf.ShelfID);
                if (saved == null)
                {
                    Debug.LogWarning($"No saved data found for shelf {shelf.ShelfID}");
                    continue;
                }
                shelf.LoadFromData(saved);
            }
        }
    }
}
